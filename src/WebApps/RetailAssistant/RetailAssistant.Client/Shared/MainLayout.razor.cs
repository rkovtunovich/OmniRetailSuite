namespace RetailAssistant.Client.Shared;

public partial class MainLayout
{
    //[Inject] private IUserPreferenceService _userPreferenceService { get; set; } = default!;

    //[Inject] private AuthenticationStateProvider _authenticationStateProvider { get; set; } = default!;

    private MudTheme _theme = new();
    private bool _isDarkMode;
    bool _drawerOpen = true;

    private void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }

    //protected override async Task OnInitializedAsync()
    //{
    //    var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
    //    var user = authState?.User;

    //    if (!user?.Identity?.IsAuthenticated ?? true)
    //        return;

    //    var userId = user?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    //    if (userId is null)
    //        return;

    //    var settings = await _userPreferenceService.GetPreferencesAsync(userId);

    //    if (settings is not null)
    //        _isDarkMode = settings.IsDarkMode;
    //}

    private async Task ChangeTheme()
    {
        _isDarkMode = !_isDarkMode;

        await Task.FromResult(0);

        //    var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        //    var user = authState?.User;

        //    if (!user?.Identity?.IsAuthenticated ?? true)
        //        return;

        //    var userId = user?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        //    if (userId is null)
        //        return;

        //    var settings = await _userPreferenceService.GetPreferencesAsync(userId);

        //    if (settings is null)
        //    {
        //        settings = new Settings
        //        {
        //            Theme = _isDarkMode ? "dark" : "light",
        //        };

        //        await _userPreferenceService.UpdatePreferencesAsync(userId, settings);
        //    }
        //    else
        //    {
        //        settings.Theme = _isDarkMode ? "dark" : "light";

        //        await _userPreferenceService.UpdatePreferencesAsync(userId, settings);
        //    }
    }
}
