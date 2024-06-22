using System.Diagnostics;
using System.Globalization;
using Helpers.IOHelper;
using Microsoft.AspNetCore.Localization;

namespace BackOffice.Client.Components;

public partial class App
{
    [CascadingParameter]
    public HttpContext? HttpContext { get; set; }

    protected override void OnInitialized()
    {
        HttpContext?.Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(
                new RequestCulture(
                    CultureInfo.CurrentCulture,
                    CultureInfo.CurrentUICulture)));
    }

    private string AppendFileVersion(string filePath)
    {
        var contentRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var path = Path.Combine(contentRootPath, filePath);

        return FileVersionHelper.GetFileVersion(path);
    }
}
