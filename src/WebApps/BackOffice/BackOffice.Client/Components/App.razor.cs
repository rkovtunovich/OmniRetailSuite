using System.Globalization;
using Helpers.IOHelper;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Localization;

namespace BackOffice.Client.Components;

public partial class App
{
    [Inject] private IUserPreferenceService _userPreferenceService { get; set; } = default!;

    [Inject] private AuthenticationStateProvider _authenticationStateProvider { get; set; } = default!;

    [Inject] private NavigationManager _navigationManager { get; set; } = default!;

    [CascadingParameter]
    public HttpContext? HttpContext { get; set; }

    protected override async Task OnInitializedAsync()
    {      
        await EnsureCorrectCulture(CultureInfo.CurrentCulture.Name);

        HttpContext?.Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(
                new RequestCulture(
                    CultureInfo.CurrentCulture,
                    CultureInfo.CurrentUICulture)));
    }

    private static string AppendFileVersion(string filePath)
    {
        var contentRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var path = Path.Combine(contentRootPath, filePath);

        return FileVersionHelper.GetFileVersion(path);
    }

    private async Task EnsureCorrectCulture(string culture)
    {
        // if user is authenticated, get user preferences
        // and check if user culture is different from the current one
        // if so, change the culture
        // and redirect to the same page
        var userId = await _authenticationStateProvider.GetUserId();

        if (userId is null)       
            return;
        
        var userPreferences = await _userPreferenceService.GetPreferencesAsync(userId);

        if (userPreferences is null)
            return;

        var userCulture = userPreferences.Language.ToString().ToLower();
        if (userCulture != culture)
            _navigationManager.NavigateTo($"/culture/{userCulture}", forceLoad: true);
    }
}
