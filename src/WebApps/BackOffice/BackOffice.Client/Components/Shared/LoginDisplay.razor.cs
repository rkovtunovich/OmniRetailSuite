using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;

namespace BackOffice.Client.Components.Shared;

public partial class LoginDisplay
{
    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    public IHttpContextAccessor HttpContextAccessor { get; set; } = default!;

    [Inject]
    NavigationManager UriHelper { get; set; } = default!;

    [Parameter]
    public string? RedirectUri { get; set; }

    public async Task StartLogin()
    {
        RedirectUri ??= UriHelper.BaseUri;

        var httpContext = HttpContextAccessor.HttpContext;

        var user = httpContext!.User;

        if (user?.Identity?.IsAuthenticated ?? false)
        {
            NavigationManager.NavigateTo(RedirectUri ?? UriHelper.BaseUri);
            return;
        }

        await httpContext.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties
        {
            RedirectUri = RedirectUri ?? UriHelper.BaseUri
        });
    }

    private string ShowUserName(AuthenticationState context)
    {
        var name = context?.User.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? "Anonymous";
        return name;
    }
}
