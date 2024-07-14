using Microsoft.Extensions.Localization;

namespace BackOffice.Client.Components.Layout;

public partial class NavMenu
{
    [Inject] private TabsService _tabsService { get; set; } = default!;

    [Inject] private IStringLocalizer<NavMenu> _localizer { get; set; } = default!;

    [Parameter]
    public EventCallback<string> OnNavLinkClick { get; set; }

    private void ClickOpenTab<TFragment>() where TFragment : ComponentBase
    {
        _tabsService.TryCreateTab<TFragment>();
    }
}
