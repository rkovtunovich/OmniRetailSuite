using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Application.Mapping.Configuration;
using ProductCatalog.Application.Services.Implementation;

namespace ProductCatalog.Application.Configuration;

public static class ConfigureAppServices
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IItemService, ProductItemService>();
        services.AddScoped<IItemParentService, ProductParentService>();
        services.AddScoped<IBrandService, ProductBrandService>();
        services.AddScoped<IItemTypeService, ProductTypeService>();

        services.AddMapping();

        return services;
    }
}
