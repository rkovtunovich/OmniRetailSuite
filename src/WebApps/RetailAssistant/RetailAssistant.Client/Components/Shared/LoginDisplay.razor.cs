using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;

namespace RetailAssistant.Client.Components.Shared;

public partial class LoginDisplay
{
    [Inject] private IStringLocalizer<LoginDisplay> _localizer { get; set; } = default!;

    private string ShowUserName(AuthenticationState context)
    {
        var name = context?.User.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? "Anonymous";
        return name;
    }
}
