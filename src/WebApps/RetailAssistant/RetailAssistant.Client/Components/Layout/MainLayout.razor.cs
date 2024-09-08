using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace RetailAssistant.Client.Components.Layout;

public partial class MainLayout : IDisposable
{
    [Inject] private IUserPreferenceService _userPreferenceService { get; set; } = default!;

    [Inject] private AuthenticationStateProvider _authenticationStateProvider { get; set; } = default!;

    [Inject] private NavigationManager _navigationManager { get; set; } = default!;

    [Inject] private IJSRuntime _jsRuntime { get; set; } = default!;

    [Inject] private IDialogService _dialogService { get; set; } = default!;

    [Inject] private IStringLocalizer<MainLayout> _localizer { get; set; } = default!;

    [Inject] private IApplicationStateService _applicationStateService { get; set; } = default!;

    private MudTheme _theme = new();
    private bool _isDarkMode;
    private bool _isPreferencesSet;
    bool _drawerOpen = true;
    private Color _appStateBadgeColor = Color.Error;

    private void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }

    protected override void OnInitialized()
    {
        _applicationStateService.OnStateChange += OnStateChange;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        await SetUserPreferences();
    }

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

    private async Task SetUserPreferences()
    {
        if (_isPreferencesSet)
            return;

        var userId = await _authenticationStateProvider.GetUserId();
        if (userId is null)
            return;

        _isPreferencesSet = true;

        var settings = await _userPreferenceService.GetPreferencesAsync(userId);

        if (settings is null)
            return;

        // if user culture is different from the current one
        // change the culture
        var userCulture = settings.Language.ToString().ToLower();
        if (userCulture != CultureInfo.CurrentCulture.Name)
        {
            await _jsRuntime.InvokeVoidAsync("blazorCulture.set", userCulture);
            _navigationManager.NavigateTo(_navigationManager.Uri, forceLoad: true);
        }

        _isDarkMode = settings.IsDarkMode;

        StateHasChanged();
    }

    #endregion

    private void OnStateChange()
    {
        _appStateBadgeColor = _applicationStateService.IsOnline ? Color.Success : Color.Error;
        StateHasChanged();
    }

    private string GetStateBadgeText()
    {
        return _applicationStateService.IsOnline ? _localizer["Online"] : _localizer["Offline"];
    }

    public void Dispose()
    {
        _applicationStateService.OnStateChange -= OnStateChange;
    }
}
