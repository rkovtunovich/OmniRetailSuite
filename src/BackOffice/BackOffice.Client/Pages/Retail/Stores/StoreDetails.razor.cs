using BackOffice.Client.Components.Base;
using BackOffice.Core.Models.Retail;
using Microsoft.AspNetCore.Components.Web;

namespace BackOffice.Client.Pages.Retail.Stores;

public partial class StoreDetails: FormBase<Store>
{
    [Inject] public IRetailService<Store> RetailService { get; set; } = null!;

    [Parameter]
    public EventCallback<string> OnSaveClick { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        _cashier_Tab_ToolbarCommands =
        [
            new()
            {
                Name = "Add",
                Icon = Icons.Material.Outlined.Add,
                Callback = EventCallback.Factory.Create<MouseEventArgs>(this, CashierTabOnAddClick),
                Tooltip = "Add"
            }
        ];
    }

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
                Tooltip = "Save"
            },
            new()
            {
                Name = "Delete",
                Icon = Icons.Material.Outlined.Delete,
                Callback = EventCallback.Factory.Create<MouseEventArgs>(this, DeleteClick),
                Tooltip = "Delete"
            },
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

    #region Cashiers Tab

    private List<ToolbarCommand> _cashier_Tab_ToolbarCommands = null!;

    private async Task CashierTabOnAddClick()
    {
        await Task.CompletedTask;
    }

    #endregion
}
