using BackOffice.Core.Models.UserPreferences;
using Microsoft.Extensions.DependencyInjection;

namespace BackOffice.Application.Services.Implementation;

public class UserPreferenceService : IUserPreferenceService
{
    private readonly IHttpService<IdentityClientSettings> _httpService;
    private readonly ILogger<UserPreferenceService> _logger;
    private readonly IdentityUriResolver _identityUriResolver;

    public UserPreferenceService([FromKeyedServices(ClientNames.IDENTITY)] IHttpService<IdentityClientSettings> httpService, ILogger<UserPreferenceService> logger, IdentityUriResolver identityUriResolver)
    {
        _logger = logger;
        _httpService = httpService;
        _identityUriResolver = identityUriResolver;
    }

    public async Task<Settings?> GetPreferencesAsync(string userId)
    {
        var uri = _identityUriResolver.GetPreferences(userId);

        var preference = await _httpService.GetAsync<UserPreference>(uri);

        return preference?.Settings;
    }

    public Task UpdatePreferencesAsync(string userId, Settings settings)
    {
        var uri = _identityUriResolver.UpdatePreferences(userId);

        return _httpService.PutAsync(uri, settings);
    }
}
