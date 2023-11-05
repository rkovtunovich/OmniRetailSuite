using BackOffice.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;

namespace BackOffice.Client.Shared;

public partial class NavMenu
{
    [Inject] private TabsService _tabsService { get; set; } = default!;

    [Parameter]
    public EventCallback<string> OnNavLinkClick { get; set; }

    private string ShowUserName(AuthenticationState context)
    {
        var name = context?.User.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? "Anonymous";
        return name;
    }

    private void ClickNavLink(string tabKey, MouseEventArgs e)
    {
        OnNavLinkClick.InvokeAsync(tabKey);
    }

    private void ClickOpenTab<TFragment>(string tabKey, MouseEventArgs e) where TFragment : ComponentBase
    {
        _tabsService.TryCreateTab<TFragment>();
    }
}
