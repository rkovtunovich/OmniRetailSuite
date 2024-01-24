using Microsoft.AspNetCore.Components.Authorization;
using RetailAssistant.Client.Helpers;

namespace RetailAssistant.Client.Shared;

public partial class LoginDisplay
{
    [Inject]
    public NavigationManager? NavigationManager { get; set; }

    [Inject]
    public IConfiguration? Configuration { get; set; }

    private string ShowUserName(AuthenticationState context)
    {
        var name = context?.User.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? "Anonymous";
        return name;
    }

    private string GetLoginUrl()
    {
        if (Configuration is null)
            throw new ArgumentNullException(nameof(Configuration));

        var identityUrl = Configuration.GetValue<string>("IdentityUrl");

        return identityUrl + "/" + IdentityUriHelper.GetRegisterUrl(NavigationManager?.Uri ?? "/");
    }
}
