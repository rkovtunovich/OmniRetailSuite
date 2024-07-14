using Microsoft.AspNetCore.Components.Forms;

namespace BackOffice.Client.Components.Pages.Retail.CashierPages;

public partial class CashierCreate : CreationFormBase<Cashier>
{
    [Inject] public IRetailService<Cashier> RetailService { get; set; } = null!;

    [Inject] private IStringLocalizer<CashierCreate> _localizer { get; set; } = default!;

    [Parameter]
    public EventCallback<string> OnSaveClick { get; set; }

    protected override void OnInitialized()
    {
        EditContext = new EditContext(Model);

        base.OnInitialized();
    }

    private async Task CreateClick()
    {
        if (!EditContext?.Validate() ?? false)
            return;

        var result = await RetailService.CreateAsync(Model);
        if (result is not null)
        {
            await OnSaveClick.InvokeAsync(null);
            CloseClick();
        }
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
                Callback = EventCallback.Factory.Create<MouseEventArgs>(this, CreateClick),
                Tooltip = _localizer["Save"]
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
