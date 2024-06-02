using Infrastructure.DataManagement.Postgres.Configuration.Settings;
using Infrastructure.SecretManagement.Abstraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Retail.Data.Config;

public static class ConfigureDbContext
{
    public static async Task<IServiceCollection> AddRetailDataInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var useOnlyInMemoryDatabase = false;
        if (configuration["UseOnlyInMemoryDatabase"] != null)
            useOnlyInMemoryDatabase = bool.Parse(configuration["UseOnlyInMemoryDatabase"]!);

        // for tests
        if (useOnlyInMemoryDatabase)
        {
            services.AddDbContext<RetailDbContext>(c => c.UseInMemoryDatabase("RetailDb"));
            return services;
        }

        var serviceProvider = services.BuildServiceProvider();
        var secretManager = serviceProvider.GetRequiredService<ISecretManager>();
        var dbOptions = serviceProvider.GetRequiredService<IOptions<DbSettings>>();

        var secretRequest = new SecretRequest
        {
            Namespace = "database",
            Path = SecretRequest.ConvertPathFromSnakeToKebabCase(dbOptions.Value.Database)
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

        services.AddDbContext<RetailDbContext>(c =>
        {
            c.UseNpgsql(dataSource);
            c.UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IReceiptRepository, ReceiptRepository>();
        services.AddScoped<ICatalogItemRepository, ProductItemRepository>();
        services.AddScoped<IRetailRepository<Cashier>, CashierRepository>();
        services.AddScoped<IRetailRepository<Store>, StoreRepository>();
        services.AddScoped<IRetailRepository<AppClientSettings>, AppClientSettingsRepository>();

        return services;
    }
}
