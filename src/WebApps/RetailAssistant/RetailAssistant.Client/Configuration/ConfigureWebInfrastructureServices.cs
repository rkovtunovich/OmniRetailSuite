using RetailAssistant.Application.Services.Implementation;

namespace RetailAssistant.Client.Configuration;

public static class ConfigureWebInfrastructureServices
{
    public static IServiceCollection AddRetailAssistantWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IdentityServerSettings>(configuration.GetSection("IdentityServerSettings"));

        var gatewayUrl = configuration[Constants.HTTP_WEB_GATEWAY] ?? throw new Exception("Gateway isn't settled in configuration");
        services.AddHttpClient(Constants.API_HTTP_CLIENT_NAME, client =>
        {
            client.BaseAddress = new Uri(gatewayUrl);
        });
        services.AddHttpClient(Constants.IDENTITY_CLIENT_NAME, client =>
        {
            client.BaseAddress = new Uri(gatewayUrl);
        });

        //services.AddHttpLogging(options =>
        //{
        //    options.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders;
        //});

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped(typeof(IHttpService<>), typeof(HttpService<>));

        return services;
    }
}
