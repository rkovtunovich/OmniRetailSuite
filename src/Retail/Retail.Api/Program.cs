using HealthChecks.UI.Client;
using Infrastructure.DataManagement.Postgres.Configuration;
using Infrastructure.DataManagement.Postgres.Extensions;
using Infrastructure.Messaging.Kafka.Configuration;
using Infrastructure.Messaging.Kafka.Configuration.Settings;
using Infrastructure.SecretManagement.Vault.Configuration;
using Infrastructure.Serialization.JsonText.Configuration;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Retail.Api.Configuration;
using Retail.Application.Configuration;
using Retail.Data;
using Retail.Data.Config;
using Steeltoe.Discovery.Client;
using Winton.Extensions.Configuration.Consul;

var builder = WebApplication.CreateBuilder(args);
builder.AddLogging();

Console.WriteLine($"Current environment: {builder.Environment.EnvironmentName}");
builder.Configuration.AddConsul($"configuration/retail/{builder.Environment.EnvironmentName.ToLower()}");

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetRequiredSection(KafkaSettings.SectionName));
var kafkaSettings = builder.Configuration.GetSection(KafkaSettings.SectionName).Get<KafkaSettings>();

builder.Services.AddHealthChecks();
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new DtoBasedRouteConvention());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddSecretManagement(builder.Configuration);
builder.Services.AddDataManagement(builder.Configuration);
await builder.Services.PrepareDatabase();
await builder.Services.AddRetailDataInfrastructure(builder.Configuration);
builder.Services.AddDiscoveryClient(builder.Configuration);
builder.Services.AddAppServices();
builder.Services.AddMessaging(kafkaSettings ?? throw new NullReferenceException(nameof(kafkaSettings)));
builder.Services.AddJsonTextSerialization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Retail API V1"));
}

await RetailDbSeeder.SeedRetailDb(app);

app.UseRouting();

app.UseHealthChecks("/_health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapControllers();

app.Run();
