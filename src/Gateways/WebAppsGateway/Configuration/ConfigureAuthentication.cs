using IdentityServer4.AccessTokenValidation;

namespace WebAppsGateway.Configuration;

public static class ConfigureAuthentication
{
    public static void AddIdentityServer(this IServiceCollection services, IConfiguration configuration)
    {
        var logger = LoggerFactory.Create(config => config.AddConsole()).CreateLogger<Program>();

        logger.LogInformation("Configuring authentication...");
        logger.LogInformation($"IdentityServer: {configuration["IdentitySettings:Authority"]}");

        services.AddAuthentication("IdentityServer")
            .AddIdentityServerAuthentication("IdentityServer", options =>
            {
                options.Authority = configuration["IdentitySettings:Authority"];
                options.RequireHttpsMetadata = false; // Set to true in production
                options.ApiName = "webappsgateway";
                options.ApiSecret = "webappsgateway-secret";
                options.SupportedTokens = SupportedTokens.Both;
            });
    }
}
