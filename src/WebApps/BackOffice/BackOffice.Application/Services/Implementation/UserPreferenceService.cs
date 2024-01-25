﻿using BackOffice.Application.Helpers;
using BackOffice.Core.Models.ExternalResources;
using BackOffice.Core.Models.UserPreferences;
using Microsoft.Extensions.DependencyInjection;

namespace BackOffice.Application.Services.Implementation;

public class UserPreferenceService : IUserPreferenceService
{
    private readonly IHttpService<IdentityResource> _httpService;
    private readonly ILogger<UserPreferenceService> _logger;

    public UserPreferenceService([FromKeyedServices(Constants.IDENTITY_CLIENT_NAME)] IHttpService<IdentityResource> httpService, ILogger<UserPreferenceService> logger)
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
