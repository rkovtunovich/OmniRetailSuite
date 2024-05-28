using Microsoft.Extensions.DependencyInjection;

namespace ProductCatalog.Application.Mapping.Configuration;

public static class ConfigureMapping
{
    public static IServiceCollection AddMapping(this IServiceCollection services)
    {
        var profiles = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Profile)))
            .ToArray();

        services.AddAutoMapper(profiles);

        return services;
    }
}
