namespace BackOffice.Client.Shared;

public partial class NavMenu
{
    [Inject] private TabsService _tabsService { get; set; } = default!;

    [Parameter]
    public EventCallback<string> OnNavLinkClick { get; set; }

    private void ClickOpenTab<TFragment>() where TFragment : ComponentBase
    {
        _tabsService.TryCreateTab<TFragment>();
    }
}
