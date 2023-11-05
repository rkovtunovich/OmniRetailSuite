using Identity.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Configuration;

public static class ConfigureDbContext
{
    public static IServiceCollection AddIdentityDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("IdentityApiConnection"));
            options.UseSnakeCaseNamingConvention();
        });

        return services;
    }
}
