using System.Globalization;
using BackOffice.Client.Localization;
using Microsoft.AspNetCore.Components.Authorization;

namespace BackOffice.Client.Extensions;

public static class WebApplicationExtension
{
    public static void UseLocalization(this WebApplication webApplication)
    {
        var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(SupportedCultures.Default)
            .AddSupportedCultures(SupportedCultures.All)
            .AddSupportedUICultures(SupportedCultures.All);

        webApplication.UseRequestLocalization(localizationOptions);
    }
}
