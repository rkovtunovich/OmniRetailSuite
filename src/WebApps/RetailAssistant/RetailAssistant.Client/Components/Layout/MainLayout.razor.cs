using Microsoft.AspNetCore.Components.Authorization;
using RetailAssistant.Core.Models.UserPreferences;

namespace RetailAssistant.Client.Components.Layout;

public partial class MainLayout
{
    [Inject] private IUserPreferenceService _userPreferenceService { get; set; } = default!;

    [Inject] private AuthenticationStateProvider _authenticationStateProvider { get; set; } = default!;

    private MudTheme _theme = new();
    private bool _isDarkMode;
    private bool _isThemeSet;
    bool _drawerOpen = true;

    private void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        await SetTheme();
    }

    private async Task ChangeTheme()
    {
        _isDarkMode = !_isDarkMode;

        await Task.FromResult(0);

        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState?.User;

        if (!user?.Identity?.IsAuthenticated ?? true)
            return;

        var userId = user?.FindFirst(c => c.Type == "sub")?.Value;
        if (userId is null)
            return;

        var settings = await _userPreferenceService.GetPreferencesAsync(userId);

        if (settings is null)
        {
            settings = new Settings
            {
                Theme = _isDarkMode ? "dark" : "light",
            };

            await _userPreferenceService.UpdatePreferencesAsync(userId, settings);
        }
        else
        {
            settings.Theme = _isDarkMode ? "dark" : "light";

            await _userPreferenceService.UpdatePreferencesAsync(userId, settings);
        }
    }

    private async Task SetTheme()
    {
        if (_isThemeSet)
            return;

        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState?.User;

        if (!user?.Identity?.IsAuthenticated ?? true)
            return;

        var userId = user?.FindFirst(c => c.Type == "sub")?.Value;
        if (userId is null)
            return;

        _isThemeSet = true;

        var settings = await _userPreferenceService.GetPreferencesAsync(userId);

        if (settings is not null)
            _isDarkMode = settings.Theme is "dark";

        StateHasChanged();
    }
}
