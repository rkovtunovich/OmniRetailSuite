namespace BackOffice.Client.Components.Pages.Retail.StorePages.TabularParts;

public partial class StoreCashiers : TabularPartBase<Cashier>
{
    [Inject] private IStringLocalizer<StoreCashiers> _localizer { get; set; } = default!;
}
