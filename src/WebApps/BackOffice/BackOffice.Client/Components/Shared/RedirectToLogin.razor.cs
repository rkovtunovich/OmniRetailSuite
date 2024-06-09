namespace BackOffice.Client.Components.Shared;

public partial class RedirectToLogin
{
    [Inject]
    public NavigationManager? NavigationManager { get; set; }

    protected override void OnInitialized()
    {
        NavigationManager?.NavigateTo($"/login?RedirectUri={Uri.EscapeDataString(NavigationManager.Uri)}", true);
    }
}
