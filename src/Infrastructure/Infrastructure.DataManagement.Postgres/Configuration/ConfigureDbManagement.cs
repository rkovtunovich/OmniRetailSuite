using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.SecretManagement.Vault.Configuration;

public static class ConfigureDbManagement
{
    public static IServiceCollection AddDataManagement(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DbSettings>(configuration.GetRequiredSection(DbSettings.SectionName));

        services.AddSingleton<IConnectionStringBuilder, ConnectionStringBuilder>();
        services.AddSingleton<IDbManager, DbManager>();

        return services;
    }
}
