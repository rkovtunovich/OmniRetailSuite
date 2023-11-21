using BackOffice.Application.Services.Abstraction.ProductCatalog;
using BackOffice.Application.Services.Implementation;
using BackOffice.Application.Services.Implementation.ProductCatalog;
using BackOffice.Client.Services;

namespace BackOffice.Client.Configuration;

public static class ConfigureBackOfficeServices
{
    public static IServiceCollection AddBackOfficeServices(this IServiceCollection services)
    {
        services.AddScoped<IProductItemService, ProductItemService>();
        services.AddScoped<IProductBrandService, ProductBrandService>();
        services.AddScoped<IProductTypeService, ProductTypeService>();
        services.AddScoped<IProductParentService, ProductParentService>();
        services.AddScoped<IUserPreferenceService, UserPreferenceService>();
        
        services.AddSingleton<TabsService>();

        return services;
    }
}
