namespace BackOffice.Client.Components.Pages.Retail.ReceiptPages.TabularParts;

public partial class ReceiptItems : TabularPartBase<ReceiptItem>
{
    [Inject] private IStringLocalizer<ReceiptItems> _localizer { get; set; } = default!;

}
