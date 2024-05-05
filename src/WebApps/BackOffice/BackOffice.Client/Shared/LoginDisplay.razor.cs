using Microsoft.AspNetCore.Components.Authorization;

namespace BackOffice.Client.Shared;

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
}
