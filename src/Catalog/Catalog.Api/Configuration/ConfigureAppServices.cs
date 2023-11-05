using Catalog.Application.Services.Abstraction;
using Catalog.Application.Services.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Api.Configuration;

public static class ConfigureAppServices
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IItemService, ItemService>();

        return services;
    }
}
