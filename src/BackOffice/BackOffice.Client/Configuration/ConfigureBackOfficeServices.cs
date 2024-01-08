using BackOffice.Application.Mapping.Configuration;
using BackOffice.Application.Services.Abstraction.ProductCatalog;
using BackOffice.Application.Services.Implementation;
using BackOffice.Application.Services.Implementation.ProductCatalog;
using BackOffice.Client.Services;
using BackOffice.Core.Models.Retail;
using Contracts.Dtos.Retail;

namespace BackOffice.Client.Configuration;

public static class ConfigureBackOfficeServices
{
    public static IServiceCollection AddBackOfficeServices(this IServiceCollection services)
    {
        services.AddMapping();

        services.AddScoped<IProductItemService, ProductItemService>();
        services.AddScoped<IProductBrandService, ProductBrandService>();
        services.AddScoped<IProductTypeService, ProductTypeService>();
        services.AddScoped<IProductParentService, ProductParentService>();
        services.AddScoped<IUserPreferenceService, UserPreferenceService>();
        
        services.AddScoped<IRetailService<Cashier>, RetailService<Cashier, CashierDto>>();
        services.AddScoped<IRetailService<Store>, RetailService<Store, StoreDto>>();

        services.AddSingleton<TabsService>();

        return services;
    }
}
