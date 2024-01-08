using BackOffice.Client.Components.Base;
using BackOffice.Core.Models.Retail;
using Microsoft.AspNetCore.Components.Web;

namespace BackOffice.Client.Pages.Retail.Stores;

public partial class StoreDetails: FormBase<Store>
{
    [Inject] public IRetailService<Store> RetailService { get; set; } = null!;

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

    private async Task DeleteClick()
    {
        var result = await RetailService.DeleteAsync(Model.Id, true);

        if (result)
            CloseClick();
    }

    #region Commands

    protected override void DefineToolbarCommands()
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
}
