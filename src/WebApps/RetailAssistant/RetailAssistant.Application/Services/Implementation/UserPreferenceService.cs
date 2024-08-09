using Blazored.LocalStorage;
using Infrastructure.Http;
using Infrastructure.Http.Clients;
using Infrastructure.Http.Uri;

namespace RetailAssistant.Application.Services.Implementation;

public class UserPreferenceService(IHttpService<IdentityClientSettings> httpService,
                                   ILogger<UserPreferenceService> logger,
                                   IdentityUriResolver identityUriResolver,
                                   IApplicationStateService applicationStateService,
                                   ILocalStorageService localStorageService) : IUserPreferenceService
{
    public async Task<Settings?> GetPreferencesAsync(string userId)
    {
        var settings = await localStorageService.GetItemAsync<Settings>(userId);

        if (!applicationStateService.IsOnline)     
            return settings;
       
        var uri = identityUriResolver.GetPreferences(userId);
        var preference = await httpService.GetAsync<UserPreference>(uri);
        if (preference is null)
            return settings;

        if(!preference.Settings.IsEquivalent(settings))
        {
            await localStorageService.SetItemAsync(userId, preference.Settings);
            settings = preference.Settings;

            logger.LogInformation("User preferences updated from server");
        }

        return settings;
    }

    public async Task UpdatePreferencesAsync(string userId, Settings settings)
    {
        var uri = identityUriResolver.UpdatePreferences(userId);

        await httpService.PutAsync(uri, settings);
    }
}
