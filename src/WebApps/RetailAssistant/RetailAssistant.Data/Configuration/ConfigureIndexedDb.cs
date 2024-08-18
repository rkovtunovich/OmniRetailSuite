using Infrastructure.DataManagement.Abstraction;
using Infrastructure.DataManagement.IndexedDb;
using Infrastructure.DataManagement.IndexedDb.Configuration;
using Infrastructure.DataManagement.IndexedDb.Configuration.Settings;
using Infrastructure.DataManagement.IndexedDb.Extensions;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

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
            await host.PrepareDatabase(new ProductCatalogDbSchema());
            await host.PrepareDatabase(new RetailDbSchema());

            logger.LogInformation("IndexedDb configured.");

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred configuring IndexedDb.");
        }
    }
 
    public static void AddIndexedDb(this IServiceCollection services)
    {
        services.AddDataManagement();
        services.AddScoped(typeof(IDbDataService<>), typeof(DbDataService<>));
    }
}
