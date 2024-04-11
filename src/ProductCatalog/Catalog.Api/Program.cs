using HealthChecks.UI.Client;
using Infrastructure.DataManagement.Postgres.Configuration;
using Infrastructure.DataManagement.Postgres.Extensions;
using Infrastructure.Messaging.Kafka;
using Infrastructure.Messaging.Kafka.Configuration;
using Infrastructure.Messaging.Kafka.Configuration.Settings;
using Infrastructure.SecretManagement.Vault.Configuration;
using Infrastructure.Serialization.JsonText.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductCatalog.Api.Configuration;
using ProductCatalog.Api.Middleware;
using ProductCatalog.Core;
using ProductCatalog.Core.Logging;
using ProductCatalog.Data;
using ProductCatalog.Data.Config;
using Steeltoe.Discovery.Client;
using Winton.Extensions.Configuration.Consul;


var builder = WebApplication.CreateBuilder(args);
builder.AddLogging();

Console.WriteLine($"Current environment: {builder.Environment.EnvironmentName}");
builder.Configuration.AddConsul($"configuration/product-catalog/{builder.Environment.EnvironmentName.ToLower()}");

builder.Services.Configure<CatalogSettings>(builder.Configuration);
builder.Services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
builder.Services.AddHealthChecks();

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetRequiredSection(KafkaSettings.SectionName));

var kafkaSettings = builder.Configuration.GetSection(KafkaSettings.SectionName).Get<KafkaSettings>() 
    ?? throw new NullReferenceException(nameof(KafkaSettings));

builder.Services.AddMessaging(kafkaSettings);
builder.Services.AddJsonTextSerialization();
builder.Services.AddDiscoveryClient(builder.Configuration);
builder.Services.AddSecretManagement(builder.Configuration);
builder.Services.AddDataManagement(builder.Configuration);
await builder.Services.PrepareDatabase();
await builder.Services.AddDataLayerDependencies(builder.Configuration);
builder.Services.AddAppServices();
builder.Services.AddMemoryCache();
builder.Services.AddSwagger();
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new DtoBasedRouteConvention());
});
builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

app.Logger.LogInformation("ProductCatalog.Api App created...");
app.Logger.LogInformation("Seeding Database...");

using (var scope = app.Services.CreateScope())
{
    var scopedProvider = scope.ServiceProvider;
    try
    {
        var catalogContext = scopedProvider.GetRequiredService<ProductDbContext>();
        await DbSeeder.SeedAsync(catalogContext, app.Logger);
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

var topicManager = app.Services.GetService<TopicManager>() ?? throw new NullReferenceException(nameof(TopicManager));
await topicManager.EnsureTopicsCreatedAsync();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseRouting();

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
// specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product catalog API V1");
});

app.UseHealthChecks("/_health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapControllers();

app.Logger.LogInformation("LAUNCHING Product Catalog Api");
app.Run();

public partial class Program { }

