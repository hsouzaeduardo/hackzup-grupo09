using System.Text.Json;
using Azure.Messaging.ServiceBus;

public class ServiceBusListener : IHostedService
{
    private readonly ILogger<ServiceBusListener> _logger;
    private ServiceBusProcessor _processor;
    private readonly BlobStorageHelper _blobStorageHelper;
    private readonly AzureOpenAIHelper _openAIHelper;
    private readonly string _connectionString;
    private readonly string _connectionStringSql;
    private readonly string _queueName;
    private readonly string _sqlqueueName;

    public ServiceBusListener(ILogger<ServiceBusListener> logger, BlobStorageHelper blobStorageHelper, AzureOpenAIHelper openAIHelper, IConfiguration configuration)
    {
        _logger = logger;
        _blobStorageHelper = blobStorageHelper;
        _openAIHelper = openAIHelper;
        _connectionString = configuration.GetConnectionString("AzureWebJobsServiceBus");
        _connectionStringSql = configuration.GetConnectionString("AzureWebJobsServiceBusSQL");
        _queueName = configuration["ServiceBus:QueueName"];
        _sqlqueueName = configuration["ServiceBus:QueueSql"];
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var client = new ServiceBusClient(_connectionString);
        _processor = client.CreateProcessor(_queueName, new ServiceBusProcessorOptions());

        _processor.ProcessMessageAsync += ProcessMessagesAsync;
        _processor.ProcessErrorAsync += ProcessErrorAsync;

        _logger.LogInformation("Starting the Service Bus listener.");
        await _processor.StartProcessingAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping the Service Bus listener.");
        await _processor.StopProcessingAsync();
        await _processor.DisposeAsync();
    }

    private async Task ProcessMessagesAsync(ProcessMessageEventArgs args)
    {
        var messageBody = args.Message.Body.ToString();
        var fileMessage = JsonSerializer.Deserialize<FileMessage>(messageBody);

        _logger.LogInformation($"Received message for file: {fileMessage.file_name} of type: {fileMessage.file_type}");

        if (fileMessage.file_type == "csv")
        {
            // Baixar o arquivo CSV como texto
            var csvContent = await _blobStorageHelper.DownloadFileAsTextAsync(fileMessage.file_name);

            // Enviar o conteúdo CSV para o Azure OpenAI para gerar comandos SQL
            var sqlCommands = await _openAIHelper.GenerateCompletionAsync(csvContent);

            // Log dos comandos SQL gerados
            _logger.LogInformation($"SQL Commands Generated: {sqlCommands}");

            //Colocar a mensagem com comandos SQL na fila de processamento
            await SendMessageSqlToQueue(sqlCommands);

        }
        else
        {
            _logger.LogWarning($"Unsupported file type: {fileMessage.file_type}");
        }

        // Complete the message
        await args.CompleteMessageAsync(args.Message);
    }

    private Task ProcessErrorAsync(ProcessErrorEventArgs args)
    {
        _logger.LogError(args.Exception, "An error occurred while processing the message.");
        return Task.CompletedTask;
    }
    private async Task SendMessageSqlToQueue(string sql)
    {
        // Cria o ServiceBusClient para se conectar ao Service Bus
        var client = new ServiceBusClient(_connectionStringSql);

        // Cria um sender para enviar mensagens para a fila de SQL
        var sender = client.CreateSender(_sqlqueueName);

        try
        {
            // Cria uma nova mensagem do Service Bus com o conteúdo SQL
            var message = new ServiceBusMessage(sql);

            // Envia a mensagem para a fila
            await sender.SendMessageAsync(message);

            // Log para confirmar que a mensagem foi enviada com sucesso
            _logger.LogInformation($"SQL message sent to queue {_sqlqueueName}: {sql}");
        }
        catch (Exception ex)
        {
            // Log de erro caso ocorra uma falha ao enviar a mensagem
            _logger.LogError(ex, "Failed to send SQL message to queue.");
        }
        finally
        {
            // Descartar o sender e o client corretamente para liberar os recursos
            await sender.DisposeAsync();
            await client.DisposeAsync();
        }
    }
}
