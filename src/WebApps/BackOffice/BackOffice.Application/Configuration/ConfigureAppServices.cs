using BackOffice.Application.Mapping.Configuration;
using BackOffice.Application.Services.Implementation;
using BackOffice.Core.Models.ProductCatalog;
using Microsoft.Extensions.DependencyInjection;

namespace BackOffice.Application.Configuration;

public static class ConfigureAppServices
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddMapping();

        services.AddScoped<IUserPreferenceService, UserPreferenceService>();

        services.AddScoped<IProductCatalogService<ProductBrand>, ProductCatalogService<ProductBrand, ProductBrandDto>>();
        services.AddScoped<IProductCatalogService<ProductType>, ProductCatalogService<ProductType, ProductTypeDto>>();
        services.AddScoped<IProductCatalogService<ProductItem>, ProductCatalogService<ProductItem, Contracts.Dtos.ProductCatalog.ProductItemDto>>();
        services.AddScoped<IProductCatalogService<ProductParent>, ProductCatalogService<ProductParent, ProductParentDto>>();

        services.AddScoped<IRetailService<Cashier>, RetailService<Cashier, CashierDto>>();
        services.AddScoped<IRetailService<Store>, RetailService<Store, StoreDto>>();

        return services;
    }
}
