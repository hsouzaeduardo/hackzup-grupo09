using Azure.Messaging.ServiceBus;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

public class ServiceBusListener : IHostedService
{
    private readonly ILogger<ServiceBusListener> _logger;
    private readonly string _serviceBusConnectionString;
    private readonly string _queueName;
    private readonly string _postgresConnectionString;
    private ServiceBusProcessor _processor;

    public ServiceBusListener(ILogger<ServiceBusListener> logger, IConfiguration configuration)
    {
        _logger = logger;
        _serviceBusConnectionString = configuration["ServiceBus:ConnectionString"];
        _queueName = configuration["ServiceBus:QueueName"];
        _postgresConnectionString = configuration.GetConnectionString("PostgresConnection");
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var client = new ServiceBusClient(_serviceBusConnectionString);
        _processor = client.CreateProcessor(_queueName, new ServiceBusProcessorOptions());

        _processor.ProcessMessageAsync += ProcessMessageAsync;
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

    private async Task ProcessMessageAsync(ProcessMessageEventArgs args)
    {
        var sqlCommand = args.Message.Body.ToString();
        _logger.LogInformation($"Received SQL command: {sqlCommand}");

        try
        {
            // Usar Dapper para executar o comando SQL no PostgreSQL
            using (IDbConnection dbConnection = new NpgsqlConnection(_postgresConnectionString))
            {
                dbConnection.Open();
                
                await dbConnection.ExecuteAsync(sqlCommand);  // Executa o SQL

                _logger.LogInformation("SQL command executed successfully.");
            }

            // Completar a mensagem no Service Bus
            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error executing SQL command: {sqlCommand}");
            await args.AbandonMessageAsync(args.Message);  // Recolocar na fila para retry
        }
    }

    private Task ProcessErrorAsync(ProcessErrorEventArgs args)
    {
        _logger.LogError(args.Exception, "Error processing message.");
        return Task.CompletedTask;
    }
}
