using Identity.Api.Infrastructure.Repositories;
using Identity.Api.Services.Abstractions;
using Identity.Api.Services.Implementations;

namespace Identity.Api.Configuration;

public static class ConfigureUserPreferences
{
    public static IServiceCollection AddUserPreferences(this IServiceCollection services)
    {
        services.AddScoped<IUserPreferenceRepository, UserPreferenceRepository>();
        services.AddScoped<IUserPreferenceService, UserPreferenceService>();

        return services;
    }
}
