using Infrastructure.Messaging.Kafka;
using Infrastructure.Messaging.Kafka.Configuration;
using Infrastructure.Messaging.Kafka.Configuration.Settings;
using Infrastructure.Serialization.JsonText.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProductCatalog.Api;
using ProductCatalog.Api.Configuration;
using ProductCatalog.Api.Middleware;
using ProductCatalog.Core;
using ProductCatalog.Core.Logging;
using ProductCatalog.Data;
using ProductCatalog.Data.Config;
using Steeltoe.Discovery.Client;


var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();
builder.Services.AddDataLayerDependencies(builder.Configuration);
builder.Services.Configure<CatalogSettings>(builder.Configuration);
builder.Services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetRequiredSection(KafkaSettings.SectionName));

var configSection = builder.Configuration.GetRequiredSection(BaseUrlConfiguration.CONFIG_NAME);
builder.Services.Configure<BaseUrlConfiguration>(configSection);
var baseUrlConfig = configSection.Get<BaseUrlConfiguration>();

var kafkaSettings = builder.Configuration.GetSection(KafkaSettings.SectionName).Get<KafkaSettings>() 
    ?? throw new NullReferenceException(nameof(KafkaSettings));

builder.Services.AddMessaging(kafkaSettings);
builder.Services.AddJsonTextSerialization();
builder.Services.AddDiscoveryClient(configSection);
builder.Services.AddAppServices();
builder.Services.AddMemoryCache();
builder.Services.AddSwagger();
builder.Services.AddControllers();
builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

app.Logger.LogInformation("CatalogApi App created...");
app.Logger.LogInformation("Seeding Database...");

using (var scope = app.Services.CreateScope())
{
    var scopedProvider = scope.ServiceProvider;
    try
    {
        var catalogContext = scopedProvider.GetRequiredService<CatalogContext>();
        await CatalogContextSeed.SeedAsync(catalogContext, app.Logger);
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
app.UseHttpsRedirection();
app.UseRouting();

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
// specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API V1");
});

app.MapControllers();

app.Logger.LogInformation("LAUNCHING CatalogApi");
app.Run();

public partial class Program { }

