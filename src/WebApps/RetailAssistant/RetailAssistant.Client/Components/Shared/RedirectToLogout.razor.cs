using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace RetailAssistant.Client.Components.Shared;

public partial class RedirectToLogout
{
    [Inject]

    public NavigationManager NavigationManager { get; set; } = null!;

    protected override void OnInitialized()
    {
        NavigationManager?.NavigateToLogout("authentication/logout");
    }
}
