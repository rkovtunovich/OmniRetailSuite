using Identity.Api.Models;

namespace Identity.Api.Services.Abstractions;

public interface IUserPreferenceService
{
    Task<UserPreference?> GetPreferencesAsync(string userId);

    Task UpdatePreferencesAsync(string userId, Settings settings);
}
