using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DataManagement.Postgres.Configuration;

public static class ConfigureDbManagement
{
    public static IServiceCollection AddDataManagement(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DbSettings>(configuration.GetRequiredSection(DbSettings.SectionName));

        services.AddSingleton<IDbManager<DbSettings>, DbManager>();

        return services;
    }
}
