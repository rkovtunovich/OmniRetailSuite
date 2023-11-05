using Identity.Api.Infrastructure.Repositories;
using Identity.Api.Models;
using Identity.Api.Services.Abstractions;

namespace Identity.Api.Services.Implementations;

public class UserPreferenceService: IUserPreferenceService
{
    private readonly IUserPreferenceRepository _repository;

    public UserPreferenceService(IUserPreferenceRepository repository)
    {
        _repository = repository;
    }

    public async Task<UserPreference?> GetPreferencesAsync(string userId)
    {
        return await _repository.GetPreferencesAsync(userId);
    }

    public async Task UpdatePreferencesAsync(string userId, Settings settings)
    {
        var preferences = await GetPreferencesAsync(userId) ?? new UserPreference { UserId = userId };
        preferences.Settings = settings;
        await _repository.UpdatePreferencesAsync(preferences);
    }
}
