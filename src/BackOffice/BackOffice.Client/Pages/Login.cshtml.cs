using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

namespace BackOffice.Client.Pages;

public class LoginModel : PageModel
{
    public IActionResult OnGet(string redirectUri)
    {
        if (redirectUri.IsNullOrEmpty())
            redirectUri = Url.Content("~/");

        if (HttpContext?.User?.Identity?.IsAuthenticated ?? false)
            Response.Redirect(redirectUri);

        return Challenge(new AuthenticationProperties
        {
            RedirectUri = redirectUri,
        },
        OpenIdConnectDefaults.AuthenticationScheme);
    }
}
