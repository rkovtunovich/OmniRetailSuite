using BackOffice.Application.Services.Implementation;
using BackOffice.Client.Services;

namespace BackOffice.Client.Configuration;

public static class ConfigureBackOfficeServices
{
    public static IServiceCollection AddBackOfficeServices(this IServiceCollection services)
    {
        //services.AddScoped<ICatalogLookupDataService<CatalogBrand>, CachedCatalogLookupDataServiceDecorator<CatalogBrand, CatalogBrandResponse>>();
        //services.AddScoped<CatalogLookupDataService<CatalogBrand, CatalogBrandResponse>>();
        //services.AddScoped<ICatalogLookupDataService<CatalogType>, CachedCatalogLookupDataServiceDecorator<CatalogType, CatalogTypeResponse>>();
        //services.AddScoped<CatalogLookupDataService<CatalogType, CatalogTypeResponse>>();
        services.AddScoped<IProductCatalogService, CatalogService>();
        services.AddScoped<IUserPreferenceService, UserPreferenceService>();
        
        services.AddSingleton<TabsService>();

        return services;
    }
}
