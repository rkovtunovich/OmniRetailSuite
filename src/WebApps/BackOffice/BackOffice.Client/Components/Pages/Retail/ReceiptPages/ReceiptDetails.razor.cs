namespace BackOffice.Client.Components.Pages.Retail.ReceiptPages;

public partial class ReceiptDetails : DetailsFormBase<Receipt>
{
    [Inject] public IRetailService<Receipt> RetailService { get; set; } = null!;

    [Inject] private IStringLocalizer<ReceiptDetails> _localizer { get; set; } = default!;

    protected override async Task<Receipt> GetModel()
    {
        return await RetailService.GetByIdAsync(Id) ?? new();
    }

    #region Commands

    protected override void DefineFormToolbarCommands()
    {
        ToolbarCommands =
        [
            new()
            {
                Name = "Close",
                Icon = Icons.Material.Outlined.Close,
                Callback = EventCallback.Factory.Create<MouseEventArgs>(this, CloseClick),
                Tooltip = _localizer["Close"]
            }
        ];
    }

    #endregion

    private List<ToolbarCommand> _itemsTabToolbarCommands = [];
}
