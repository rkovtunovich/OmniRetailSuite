using RetailAssistant.Client;
using RetailAssistant.Client.Model;

namespace RetailAssistant.Client.Configuration;

public static class ConfigureWebInfrastructureServices
{
    public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IdentityServerSettings>(configuration.GetSection("IdentityServerSettings"));

        services.AddHttpClient(Constants.API_HTTP_CLIENT_NAME, client =>
        {
            client.BaseAddress = new Uri(configuration["ApiUrl"] ?? throw new Exception("ApiUrl isn't settled in configuration"));
        });
        services.AddHttpClient(Constants.IDENTITY_CLIENT_NAME, client =>
        {
            client.BaseAddress = new Uri(configuration["IdentityUrl"] ?? throw new Exception("IdentityUrl isn't settled in configuration"));
        });

        //services.AddHttpLogging(options =>
        //{
        //    options.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders;
        //});

        //services.AddScoped<ITokenService, TokenService>();
        //services.AddKeyedScoped<IHttpService, CatalogHttpService>(Constants.API_HTTP_CLIENT_NAME);
        //services.AddKeyedScoped<IHttpService, IdentityHttpService>(Constants.IDENTITY_CLIENT_NAME);

        return services;
    }
}
