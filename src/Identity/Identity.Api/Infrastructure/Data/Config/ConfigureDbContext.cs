using Infrastructure.DataManagement.Postgres.Configuration.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Identity.Api.Infrastructure.Data.Config;

public static class ConfigureDbContext
{
    public static async Task<IServiceCollection> AddIdentityDbContext(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var secretManager = serviceProvider.GetRequiredService<ISecretManager>();
        var dbOptions = serviceProvider.GetRequiredService<IOptions<DbSettings>>();

        var secretRequest = new SecretRequest
        {
            Namespace = "database",
            Path = "identity",
        };

        var credentials = await secretManager.GetSecretAsync(secretRequest)
            ?? throw new InvalidOperationException("Database credentials are empty.");

        var connectionStringBuilder = new NpgsqlConnectionStringBuilder
        {
            Host = dbOptions.Value.Host,
            Port = dbOptions.Value.Port,
            Username = credentials["username"],
            Password = credentials["password"],
            Database = dbOptions.Value.Database
        };

        var builder = new NpgsqlDataSourceBuilder(connectionString: connectionStringBuilder.ConnectionString)
            .EnableDynamicJson();

        var dataSource = builder.Build();

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(dataSource);
            options.UseSnakeCaseNamingConvention();
        });

        return services;
    }
}
