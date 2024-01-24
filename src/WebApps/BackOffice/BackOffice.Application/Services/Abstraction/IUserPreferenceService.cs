using BackOffice.Core.Models.UserPreferences;

namespace BackOffice.Application.Services.Abstraction;

public interface IUserPreferenceService
{
    Task<Settings?> GetPreferencesAsync(string userId);

    Task UpdatePreferencesAsync(string userId, Settings settings);
}
