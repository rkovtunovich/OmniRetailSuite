using Microsoft.Extensions.Localization;

namespace RetailAssistant.Client.Components.Layout;

public partial class NavMenu
{
    [Inject] private IStringLocalizer<NavMenu> _localizer { get; set; } = default!;
}
