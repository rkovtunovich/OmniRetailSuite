﻿using BackOffice.Client.Components.Base;
using BackOffice.Core.Models.Retail;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;

namespace BackOffice.Client.Pages.Retail.Cashiers;

public partial class CashierCreate: FormBase<Cashier>
{
    [Inject] public IRetailService<Cashier> RetailService { get; set; } = null!;

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
                Tooltip = "Save"
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
