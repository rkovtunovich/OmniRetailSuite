using BackOffice.Application.Services.Abstraction.ProductCatalog;
using BackOffice.Application.Services.Abstraction.Retail;
using BackOffice.Application.Services.Implementation;
using BackOffice.Application.Services.Implementation.ProductCatalog;
using BackOffice.Application.Services.Implementation.Retail;
using BackOffice.Client.Services;
using BackOffice.Core.Models.Retail;

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
        
        services.AddScoped<IRetailService<Cashier>, CashierService>();

        services.AddSingleton<TabsService>();

        return services;
    }
}
