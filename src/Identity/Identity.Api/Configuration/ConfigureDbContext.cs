using Identity.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Identity.Api.Configuration;

public static class ConfigureDbContext
{
    public static IServiceCollection AddIdentityDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var builder = new NpgsqlDataSourceBuilder(connectionString: configuration.GetConnectionString("IdentityApiConnection"))
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
