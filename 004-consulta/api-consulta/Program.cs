using api_consulta.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);


// Adiciona o repositório à injeção de dependência
builder.Services.AddSingleton<FamiliasRepository>(sp =>
    new FamiliasRepository(builder.Configuration.GetConnectionString("PostgresConnection")));

// Adiciona serviços de controle da API

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.TypeInfoResolver = JsonContext.Default;
    });

builder.Services.AddApplicationInsightsTelemetry(new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions
{
    ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]
});

var app = builder.Build();

// Define os endpoints da API
app.MapControllers();

app.Run();
