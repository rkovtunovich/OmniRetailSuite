using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Application.Services.Abstraction;
using ProductCatalog.Application.Services.Implementation;

namespace ProductCatalog.Api.Configuration;

public static class ConfigureAppServices
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IItemService, ItemService>();

        return services;
    }
}
