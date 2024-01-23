using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Retail.Data.Config;

public static class ConfigureDbContext
{
    public static IServiceCollection AddRetailDataInfrastructure(this IServiceCollection services, IConfiguration configuration)
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

        services.AddDbContext<RetailDbContext>(c =>
        {
            c.UseNpgsql(configuration.GetConnectionString("RetailDbConnection"));
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
