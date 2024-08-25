using Infrastructure.DataManagement.IndexedDb.Configuration.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DataManagement.IndexedDb.Configuration;

public static class ConfigureDbManagement
{
    public static IServiceCollection AddDataManagement(this IServiceCollection services)
    {
        services.AddSingleton<IDbManager<DbSchema>, DbManager>();

        return services;
    }
}
