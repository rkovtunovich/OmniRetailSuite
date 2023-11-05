using Catalog.Core.Entities.CatalogAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
        return new List<Brand>
            {
                new("Azure"),
                new(".NET"),
                new("Visual Studio"),
                new("SQL Server"),
                new("Other")
            };
    }

    static IEnumerable<ItemType> GetPreconfiguredCatalogTypes()
    {
        return new List<ItemType>
            {
                new("Mug"),
                new("T-Shirt"),
                new("Sheet"),
                new("USB Memory Stick")
            };
    }

    static IEnumerable<Item> GetPreconfiguredItems()
    {
        return new List<Item>
            {
                new(12,2, ".NET Bot Black Sweatshirt", ".NET Bot Black Sweatshirt", 19.5M,  "http://catalogbaseurltobereplaced/images/products/1.png"),
                new(11,2, ".NET Black & White Mug", ".NET Black & White Mug", 8.50M, "http://catalogbaseurltobereplaced/images/products/2.png"),
                new(12,5, "Prism White T-Shirt", "Prism White T-Shirt", 12,  "http://catalogbaseurltobereplaced/images/products/3.png"),
                new(12,2, ".NET Foundation Sweatshirt", ".NET Foundation Sweatshirt", 12, "http://catalogbaseurltobereplaced/images/products/4.png"),
                new(13,5, "Roslyn Red Sheet", "Roslyn Red Sheet", 8.5M, "http://catalogbaseurltobereplaced/images/products/5.png"),
                new(12,2, ".NET Blue Sweatshirt", ".NET Blue Sweatshirt", 12, "http://catalogbaseurltobereplaced/images/products/6.png"),
                new(12,5, "Roslyn Red T-Shirt", "Roslyn Red T-Shirt",  12, "http://catalogbaseurltobereplaced/images/products/7.png"),
                new(12,5, "Kudu Purple Sweatshirt", "Kudu Purple Sweatshirt", 8.5M, "http://catalogbaseurltobereplaced/images/products/8.png"),
                new(11,5, "Cup<T> White Mug", "Cup<T> White Mug", 12, "http://catalogbaseurltobereplaced/images/products/9.png"),
                new(13,2, ".NET Foundation Sheet", ".NET Foundation Sheet", 12, "http://catalogbaseurltobereplaced/images/products/10.png"),
                new(13,2, "Cup<T> Sheet", "Cup<T> Sheet", 8.5M, "http://catalogbaseurltobereplaced/images/products/11.png"),
                new(12,5, "Prism White TShirt", "Prism White TShirt", 12, "http://catalogbaseurltobereplaced/images/products/12.png")
            };
    }
}
