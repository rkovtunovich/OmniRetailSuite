namespace RetailAssistant.Client.Configuration;

public static class ConfigureRetailServices
{
    public static IServiceCollection AddBackOfficeServices(this IServiceCollection services)
    {
        //services.AddScoped<ICatalogService, CatalogService>();
        //services.AddScoped<IUserPreferenceService, UserPreferenceService>();

        //services.AddSingleton<TabsService>();

        return services;
    }
}
