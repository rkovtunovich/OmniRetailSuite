using BackOffice.Client.Localization;
using Microsoft.AspNetCore.Localization;

namespace BackOffice.Client.Endpoints;

public static class MapCultureChangingEndpointExtension
{
    public static void MapCultureChangingEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/culture/{culture}", (string culture, HttpContext context) =>
        {
            if (string.IsNullOrWhiteSpace(culture))
                culture = SupportedCultures.Default;

            if (!SupportedCultures.All.Contains(culture))
                culture = SupportedCultures.Default;

            context.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(
                    new RequestCulture(
                        culture,
                        culture)));

            return TypedResults.Redirect("/");
        });
    }
}
