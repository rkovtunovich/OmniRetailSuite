using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;

namespace BackOffice.Client.Configuration;

public static class MapAuthenticationEndpointsExtension
{
    public static void MapAuthenticationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/login", (string? redirectUri, HttpContext context) =>
        {
            if (string.IsNullOrWhiteSpace(redirectUri))
                redirectUri = "/";

            var properties = new AuthenticationProperties
            {
                RedirectUri = redirectUri
            };

            return TypedResults.Challenge(properties, [OpenIdConnectDefaults.AuthenticationScheme]);
        });

        endpoints.MapGet("/logout", () =>
        {
            return TypedResults.SignOut(new AuthenticationProperties
            {
                RedirectUri = "/"
            },
                [OpenIdConnectDefaults.AuthenticationScheme,
        CookieAuthenticationDefaults.AuthenticationScheme]);
        });
    }
}
