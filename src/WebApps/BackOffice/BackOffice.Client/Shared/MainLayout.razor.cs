﻿using BackOffice.Core.Models.UserPreferences;
using Microsoft.AspNetCore.Components.Authorization;

namespace BackOffice.Client.Shared;

public partial class MainLayout
{
    [Inject] private IUserPreferenceService _userPreferenceService { get; set; } = default!;

    [Inject] private AuthenticationStateProvider _authenticationStateProvider { get; set; } = default!;

    [Inject] private TabsService _tabsService { get; set; } = default!;

    private MudTheme _theme = new();
    private bool _isDarkMode;
    bool _drawerOpen = true;

    private void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }

    protected override async Task OnInitializedAsync()
    {
        var userId = await GetUserId();
        if (userId is null)
            return;

        var settings = await _userPreferenceService.GetPreferencesAsync(userId);

        if (settings is not null)     
            _isDarkMode = settings.IsDarkMode;
        
        _tabsService.OnTabChanged += CallRequestRefresh;
    }

    private async Task ChangeTheme()
    {
        _isDarkMode = !_isDarkMode;

        var userId = await GetUserId();
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

    #region Tabs

    protected override void OnAfterRender(bool firstRender)
    {
        // for tabs that are created on the first render
        _tabsService.ActivateCurrentTab();
    }

    #endregion

    private async  Task<string?> GetUserId()
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState?.User;
        if (user is null)
            return null;

        if (!user.Identity?.IsAuthenticated ?? true)
            return null;

         return user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    }
}
