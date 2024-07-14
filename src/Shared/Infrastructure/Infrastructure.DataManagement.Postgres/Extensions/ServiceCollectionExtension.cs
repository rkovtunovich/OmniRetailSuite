using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DataManagement.Postgres.Extensions;

public static class ServiceCollectionExtension
{
    public static async Task PrepareDatabase(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();

        var logger = serviceProvider.GetRequiredService<ILogger<IDbManager>>();

        logger.LogInformation("Prepare Database for seeding...");

        try
        {
            var dbManager = serviceProvider.GetRequiredService<IDbManager>();
            var dbOptions = serviceProvider.GetRequiredService<IOptions<DbSettings>>();

            await dbManager.EnsureDatabaseExists(dbOptions.Value.Database);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred preparing the DB for seeding.");
        }
    }
}
