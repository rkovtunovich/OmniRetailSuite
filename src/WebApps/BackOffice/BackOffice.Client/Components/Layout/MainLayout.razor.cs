using Microsoft.AspNetCore.Components.Authorization;

namespace BackOffice.Client.Components.Layout;

public partial class MainLayout
{
    [Inject] private IUserPreferenceService _userPreferenceService { get; set; } = default!;

    [Inject] private AuthenticationStateProvider _authenticationStateProvider { get; set; } = default!;

    [Inject] private TabsService _tabsService { get; set; } = default!;

    [Inject] private IDialogService _dialogService { get; set; } = default!;

    [Inject] private IStringLocalizer<MainLayout> _localizer { get; set; } = default!;

    private MudTheme _theme = new();
    private bool _isDarkMode;
    bool _drawerOpen = true;

    private void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }

    protected override async Task OnInitializedAsync()
    {
        var userId = await _authenticationStateProvider.GetUserId();
        if (userId is null)
            return;

        var settings = await _userPreferenceService.GetPreferencesAsync(userId);

        if (settings is not null)
            _isDarkMode = settings.IsDarkMode;

        _tabsService.OnTabChanged += CallRequestRefresh;
    }

    #region Tabs

    protected override void OnAfterRender(bool firstRender)
    {
        // for tabs that are created on the first render
        _tabsService.ActivateCurrentTab();
    }

    #endregion

    #region App Settings

    private void OpenAppSettings()
    {
        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true,
            BackdropClick = false
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

    #endregion
}
