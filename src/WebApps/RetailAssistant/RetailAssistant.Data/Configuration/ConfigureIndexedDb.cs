using Infrastructure.DataManagement.Abstraction;
using Infrastructure.DataManagement.IndexedDb;
using Infrastructure.DataManagement.IndexedDb.Configuration;
using Infrastructure.DataManagement.IndexedDb.Configuration.Settings;
using Infrastructure.DataManagement.IndexedDb.Extensions;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace RetailAssistant.Data.Configuration;

public static class ConfigureIndexedDb
{
    public static async Task ConfigureIndexedDbAsync(this WebAssemblyHost host)
    {
        var services = host.Services;

        var logger = services.GetRequiredService<ILogger<IDbManager<DbSchema>>>();

        logger.LogInformation("Configuring IndexedDb...");

        try
        {
            await host.PrepareDatabase(services.GetRequiredService<IOptions<ProductCatalogDbSchema>>().Value);
            await host.PrepareDatabase(services.GetRequiredService<IOptions<RetailDbSchema>>().Value);

            logger.LogInformation("IndexedDb configured.");

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred configuring IndexedDb.");
        }
    }
 
    public static void AddIndexedDb(this IServiceCollection services)
    {
        services.AddSingleton(Options.Create(new ProductCatalogDbSchema()));
        services.AddSingleton(Options.Create(new RetailDbSchema()));

        services.AddDataManagement();
        services.AddScoped(typeof(IDbDataService<>), typeof(DbDataService<>));
        services.AddScoped(typeof(IApplicationRepository<,>), typeof(ApplicationRepository<,>));
    }
}
