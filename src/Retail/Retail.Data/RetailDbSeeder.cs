using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Retail.Data;

public class RetailDbSeeder
{
    public static async Task SeedAsync(RetailDbContext retailDbContext, ILogger logger, int retry = 0)
    {
        var retryForAvailability = retry;
        try
        {
            if (retailDbContext.Database.IsNpgsql())
            {
                retailDbContext.Database.Migrate();
            }

            if (!await retailDbContext.Cashiers.AnyAsync())
            {
                await retailDbContext.Cashiers.AddRangeAsync(GetPreconfiguredCashiers());
                await retailDbContext.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            if (retryForAvailability >= 10) throw;

            retryForAvailability++;

            logger.LogError(ex.Message);
            await SeedAsync(retailDbContext, logger, retryForAvailability);
            throw;
        }
    }

    private static List<Cashier> GetPreconfiguredCashiers()
    {
        return
            [
                new() { Name = "default", CreatedAt = DateTimeOffset.Now }
            ];
    }

    public static async Task SeedRetailDb(IHost app)
    {
        using var scope = app.Services.CreateScope();
        var scopedProvider = scope.ServiceProvider;
        var logger = scopedProvider.GetRequiredService<ILogger<RetailDbSeeder>>();
        try
        {
            var catalogContext = scopedProvider.GetRequiredService<RetailDbContext>();
            await SeedAsync(catalogContext, logger);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred seeding the DB.");
        }
    }
}
