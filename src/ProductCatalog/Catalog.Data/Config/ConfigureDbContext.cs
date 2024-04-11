using Infrastructure.DataManagement.Postgres.Configuration.Settings;
using Infrastructure.SecretManagement.Abstraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;

namespace ProductCatalog.Data.Config;

public static class ConfigureDbContext
{
    public static async Task<IServiceCollection> AddDataLayerDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var useOnlyInMemoryDatabase = false;
        if (configuration["UseOnlyInMemoryDatabase"] is not null)
            useOnlyInMemoryDatabase = bool.Parse(configuration["UseOnlyInMemoryDatabase"]!);

        // for tests
        if (useOnlyInMemoryDatabase)
        {
            services.AddDbContext<ProductDbContext>(c => c.UseInMemoryDatabase("product_catalog"));
            return services;
        }

        var serviceProvider = services.BuildServiceProvider();
        var secretManager = serviceProvider.GetRequiredService<ISecretManager>();
        var dbOptions = serviceProvider.GetRequiredService<IOptions<DbSettings>>();

        var secretRequest = new SecretRequest
        {
            Namespace = "database",
            Path = dbOptions.Value.Database.Replace('_', '-'),
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

        services.AddDbContext<ProductDbContext>(c =>
        {
            c.UseNpgsql(dataSource);
            c.UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IItemParentRepository, ItemParentRepository>();
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<IItemTypeRepository, ItemTypeRepository>();

        return services;
    }
}
