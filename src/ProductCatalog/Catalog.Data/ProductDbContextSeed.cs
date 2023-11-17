using ProductCatalog.Core.Entities.ProductAggregate;

namespace ProductCatalog.Data;

public class ProductDbContextSeed
{
    public static async Task SeedAsync(ProductDbContext catalogContext, ILogger logger, int retry = 0)
    {
        var retryForAvailability = retry;
        try
        {
            if (catalogContext.Database.IsNpgsql())
            {
                catalogContext.Database.Migrate();
            }

            if (!await catalogContext.Brands.AnyAsync())
            {
                await catalogContext.Brands.AddRangeAsync(GetPreconfiguredProductBrands());
                await catalogContext.SaveChangesAsync();
            }

            if (!await catalogContext.ItemTypes.AnyAsync())
            {
                await catalogContext.ItemTypes.AddRangeAsync(GetPreconfiguredProductTypes());
                await catalogContext.SaveChangesAsync();
            }

            if (!await catalogContext.Items.AnyAsync())
            {
                await catalogContext.Items.AddRangeAsync(GetPreconfiguredItems());
                await catalogContext.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            if (retryForAvailability >= 10) throw;

            retryForAvailability++;

            logger.LogError(ex.Message);
            await SeedAsync(catalogContext, logger, retryForAvailability);
            throw;
        }
    }

    static IEnumerable<Brand> GetPreconfiguredProductBrands()
    {
        return [];
    }

    static IEnumerable<ItemType> GetPreconfiguredProductTypes()
    {
        return [];
    }

    static IEnumerable<Item> GetPreconfiguredItems()
    {
        return [];
    }
}
