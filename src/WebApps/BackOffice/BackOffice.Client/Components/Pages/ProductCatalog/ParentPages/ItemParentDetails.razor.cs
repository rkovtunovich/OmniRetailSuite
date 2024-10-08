﻿using AutoMapper;
using BackOffice.Core.Models.ProductCatalog;

namespace BackOffice.Client.Components.Pages.ProductCatalog.ParentPages;

public partial class ItemParentDetails : DetailsFormBase<ProductParent>
{
    [Inject] public IProductCatalogService<ProductParent> ProductParentService { get; set; } = null!;

    [Inject] public IMapper Mapper { get; set; } = null!;

    [Inject] private IStringLocalizer<ItemParentDetails> _localizer { get; set; } = default!;

    [Parameter]
    public EventCallback<string> OnSaveClick { get; set; }

    private IList<ProductParent> _allProductParents = [];

    protected override async Task<ProductParent> GetModel()
    {
        return await ProductParentService.GetByIdAsync(Id) ?? new();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _allProductParents = await ProductParentService.GetAllAsync();

            CallRequestRefresh();
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

    private async Task SaveClick()
    {
        if (!EditContext?.Validate() ?? false)
            return;

        var result = await ProductParentService.UpdateAsync(Model);
        if (result)
        {
            await OnSaveClick.InvokeAsync(null);
            CloseClick();
        }
    }

    private async Task DeleteClick()
    {
        await ProductParentService.DeleteAsync(Model.Id, true);

        CloseClick();
    }

    #endregion

    private async Task OnParentChanged(HierarchySelectModel selectModel)
    {
        Model.ParentId = selectModel?.Id;
        if (Model.ParentId is null || Model.ParentId == Guid.Empty)
            Model.Parent = null;
        else
            Model.Parent = await ProductParentService.GetByIdAsync(Model.ParentId.Value);

    }
}
