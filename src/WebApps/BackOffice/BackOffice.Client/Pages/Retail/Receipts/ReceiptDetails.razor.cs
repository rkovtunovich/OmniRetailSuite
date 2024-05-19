using Microsoft.AspNetCore.Components.Web;

namespace BackOffice.Client.Pages.Retail.Receipts;

public partial class ReceiptDetails : DetailsFormBase<Receipt>
{
    [Inject] public IRetailService<Receipt> RetailService { get; set; } = null!;


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
                Tooltip = "Close"
            }
        ];
    }

    #endregion

    private List<ToolbarCommand> _itemsTabToolbarCommands = [];
}
