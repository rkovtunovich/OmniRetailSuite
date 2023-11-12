using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Data;
using ProductCatalog.Data.Repositories;

namespace ProductCatalog.Data.Config;

public static class ConfigureDbContext
{
    public static IServiceCollection AddDataLayerDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var useOnlyInMemoryDatabase = false;
        if (configuration["UseOnlyInMemoryDatabase"] != null)
            useOnlyInMemoryDatabase = bool.Parse(configuration["UseOnlyInMemoryDatabase"]!);

        // for tests
        if (useOnlyInMemoryDatabase)
        {
            services.AddDbContext<CatalogContext>(c => c.UseInMemoryDatabase("Catalog"));
            return services;
        }

        services.AddDbContext<CatalogContext>(c =>
        {
            c.UseNpgsql(configuration.GetConnectionString("CatalogConnection"));
            c.UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IItemParentRepository, ItemParentRepository>();
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<IItemTypeRepository, ItemTypeRepository>();

        return services;
    }
}
