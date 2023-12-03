﻿using BackOffice.Application.Services.Abstraction.ProductCatalog;
using BackOffice.Client.Services;
using BackOffice.Core.Models.ProductCatalog;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;

namespace BackOffice.Client.Pages.ProductCatalog.Type;

public partial class TypeCreate
{
    [Inject] private TabsService _tabsService { get; set; } = null!;

    [Inject] public IProductTypeService ProductTypeService { get; set; } = null!;

    [Parameter]
    public EventCallback<string> OnSaveClick { get; set; }

    private ProductType _type = new();

    private EditContext? _editContext;

    private List<ToolbarCommand> _toolbarCommands = null!;

    protected override void OnInitialized()
    {
        _editContext = new EditContext(_type);

        DefineToolbarCommands();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task CreateClick()
    {
        if (!_editContext?.Validate() ?? false)
            return;

        var result = await ProductTypeService.CreateTypeAsync(_type);
        if (result is not null)
        {
            await OnSaveClick.InvokeAsync(null);
            CloseClick();
        }
    }

    private void CloseClick()
    {
        _tabsService.RemoveTab(_tabsService.Tabs?.ActivePanel);
    }

    #region Commands

    private void DefineToolbarCommands()
    {
        _toolbarCommands =
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
