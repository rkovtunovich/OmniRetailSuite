namespace Catalog.Data;

public class CatalogContextSeed
{
    public static async Task SeedAsync(CatalogContext catalogContext, ILogger logger, int retry = 0)
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
                await catalogContext.Brands.AddRangeAsync(GetPreconfiguredCatalogBrands());
                await catalogContext.SaveChangesAsync();
            }

            if (!await catalogContext.ItemTypes.AnyAsync())
            {
                await catalogContext.ItemTypes.AddRangeAsync(GetPreconfiguredCatalogTypes());
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

    static IEnumerable<Brand> GetPreconfiguredCatalogBrands()
    {
        return [];
    }

    static IEnumerable<ItemType> GetPreconfiguredCatalogTypes()
    {
        return [];
    }

    static IEnumerable<Item> GetPreconfiguredItems()
    {
        return [];
    }
}
