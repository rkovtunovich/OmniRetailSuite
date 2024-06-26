using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;

namespace RetailAssistant.Client.Components.Layout;

public partial class MainLayout
{
    [Inject] private IUserPreferenceService _userPreferenceService { get; set; } = default!;

    [Inject] private AuthenticationStateProvider _authenticationStateProvider { get; set; } = default!;

    [Inject] private IDialogService _dialogService { get; set; } = default!;

    [Inject] private IStringLocalizer<MainLayout> _localizer { get; set; } = default!;

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

    #region App Settings

    private void OpenAppSettings()
    {
        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true,
            DisableBackdropClick = true
        };

        var parameters = new DialogParameters
        {
            { "OnThemeChanged", EventCallback.Factory.Create<bool>(this, ChangeThemeInSettings) }
        };

        _dialogService.Show<AppSettingsDialog>(@_localizer["Settings"], parameters, options);
    }

    private void ChangeThemeInSettings(bool isDarkMode)
    {
        _isDarkMode = isDarkMode;
    }

    private async Task SetTheme()
    {
        if (_isThemeSet)
            return;

        var userId = await _authenticationStateProvider.GetUserId();
        if (userId is null)
            return;

        _isThemeSet = true;

        var settings = await _userPreferenceService.GetPreferencesAsync(userId);

        if (settings is not null)
            _isDarkMode = settings.IsDarkMode;

        StateHasChanged();
    }

    #endregion
}
