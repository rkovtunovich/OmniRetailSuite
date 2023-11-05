﻿using BackOffice.Core.Models.UserPreferences;

namespace BackOffice.Client.Services.Implementation;

public class UserPreferenceService : IUserPreferenceService
{
    private readonly IHttpService _httpService;
    private readonly ILogger<CatalogService> _logger;

    public UserPreferenceService([FromKeyedServices(Constants.IDENTITY_CLIENT_NAME)] IHttpService httpService, ILogger<CatalogService> logger)
    {
        _logger = logger;
        _httpService = httpService;
    }

    public async Task<Settings?> GetPreferencesAsync(string userId)
    {
        var uri = IdentityUriHelper.GetPreferences(userId);

        var preference = await _httpService.GetAsync<UserPreference>(uri);

        return preference?.Settings;
    }

    public Task UpdatePreferencesAsync(string userId, Settings settings)
    {
        var uri = IdentityUriHelper.UpdatePreferences(userId);

        return _httpService.PutAsync(uri, settings);
    }
}
