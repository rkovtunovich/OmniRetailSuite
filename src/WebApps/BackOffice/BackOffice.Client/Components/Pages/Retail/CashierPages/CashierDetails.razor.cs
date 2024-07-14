namespace BackOffice.Client.Components.Pages.Retail.CashierPages;

public partial class CashierDetails: DetailsFormBase<Cashier>
{
    [Inject] public IRetailService<Cashier> RetailService { get; set; } = null!;

    [Inject] private IStringLocalizer<CashierDetails> _localizer { get; set; } = default!;

    [Parameter]
    public EventCallback<string> OnSaveClick { get; set; }

    private async Task SaveClick()
    {
        if (!EditContext?.Validate() ?? false)
            return;

        var result = await RetailService.UpdateAsync(Model);
        if (result)
        {
            await OnSaveClick.InvokeAsync(null);
            CloseClick();
        }
    }

    protected override async Task<Cashier> GetModel()
    {
        return await RetailService.GetByIdAsync(Id) ?? new();
    }

    private async Task DeleteClick()
    {
        var result = await RetailService.DeleteAsync(Model.Id, true);

        if (result)
            CloseClick();
    }

    #region Commands

    protected override void DefineFormToolbarCommands()
    {
        ToolbarCommands =
        [
            new()
            {
                Name = "Save",
                Icon = Icons.Material.Outlined.Save,
                Callback = EventCallback.Factory.Create<MouseEventArgs>(this, SaveClick),
                Tooltip = _localizer["Save"]
            },
            new()
            {
                Name = "Delete",
                Icon = Icons.Material.Outlined.Delete,
                Callback = EventCallback.Factory.Create<MouseEventArgs>(this, DeleteClick),
                Tooltip = _localizer["Delete"]
            },
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
}
