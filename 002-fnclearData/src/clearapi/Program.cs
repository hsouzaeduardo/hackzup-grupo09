using Microsoft.AspNetCore.Builder;


var builder = WebApplication.CreateBuilder(args);

var storageConnectionString = builder.Configuration.GetConnectionString("AzureBlobStorage");
builder.Services.AddSingleton(new BlobStorageHelper(storageConnectionString));


// Adicionar a API Key e o endpoint da Azure OpenAI
var openAIEndpoint = builder.Configuration["AzureOpenAI:Endpoint"];
var openAIKey = builder.Configuration["AzureOpenAI:ApiKey"];
var deploymentName = builder.Configuration["AzureOpenAI:DeploymentName"];
var serviceBusConnection = builder.Configuration.GetConnectionString("AzureWebJobsServiceBus");
var criptKey = builder.Configuration["Cripto:Key"];
var criptKeyIV = builder.Configuration["Cripto:IV"];

builder.Services.AddSingleton(new AzureOpenAIHelper(openAIEndpoint, openAIKey, deploymentName, criptKey, criptKeyIV));

builder.Services.AddHealthChecks();

builder.Services.AddControllers();

builder.Services.AddHostedService<ServiceBusListener>();
builder.Services.AddApplicationInsightsTelemetry(new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions
{
    ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"]
});

var app = builder.Build();

app.MapHealthChecks("/health");

app.MapControllers();

app.Run();
