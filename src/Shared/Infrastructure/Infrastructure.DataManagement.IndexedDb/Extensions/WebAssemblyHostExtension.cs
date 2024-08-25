using Infrastructure.DataManagement.IndexedDb.Configuration.Settings;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DataManagement.IndexedDb.Extensions;

public static class WebAssemblyHostExtension
{
    public static async Task PrepareDatabase(this WebAssemblyHost host, DbSchema dbSchema)
    {
        var services = host.Services;

        var logger = services.GetRequiredService<ILogger<IDbManager<DbSchema>>>();

        logger.LogInformation($"Prepare {dbSchema.Name} for seeding...");

        try
        {
            var dbManager = services.GetRequiredService<IDbManager<DbSchema>>();

            await dbManager.EnsureDatabaseExists(dbSchema);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred preparing the DB for seeding.");
        }
    }
}
