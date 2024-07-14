using Microsoft.Extensions.DependencyInjection;

namespace BackOffice.Application.Services.Implementation;

public class UserPreferenceService([FromKeyedServices(ClientNames.IDENTITY)] IHttpService<IdentityClientSettings> httpService,
                                   IdentityUriResolver identityUriResolver) : IUserPreferenceService
{
    public async Task<Settings?> GetPreferencesAsync(string userId)
    {
        var uri = identityUriResolver.GetPreferences(userId);

        var preference = await httpService.GetAsync<UserPreference>(uri);

        return preference?.Settings;
    }

    public Task UpdatePreferencesAsync(string userId, Settings settings)
    {
        var uri = identityUriResolver.UpdatePreferences(userId);

        return httpService.PutAsync(uri, settings);
    }
}
