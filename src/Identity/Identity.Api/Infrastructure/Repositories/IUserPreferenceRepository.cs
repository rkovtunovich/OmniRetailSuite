using Identity.Api.Models;

namespace Identity.Api.Infrastructure.Repositories;

public interface IUserPreferenceRepository
{
    Task<UserPreference?> GetPreferencesAsync(string userId);

    Task UpdatePreferencesAsync(UserPreference preferences);
}

