﻿using BackOffice.Client.Pages.ProductCatalog.Brand;
using BackOffice.Core.Models.ProductCatalog;

namespace BackOffice.Client.Pages.ProductCatalog.Type;

public partial class TypeList : ListBase<ProductType>
{
    [Inject] public IProductCatalogService<ProductType> ProductTypeService { get; set; } = null!;

    private DataGridEditMode _editMode = DataGridEditMode.Form;
    private DataGridEditTrigger _editTrigger = DataGridEditTrigger.Manual;
    private DialogOptions _dialogOptions = new() { DisableBackdropClick = true };

    #region Overrides

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            ProductTypeService.OnChanged += OnCatalogTypeChanged;

            await ReloadCatalogTypes();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    #endregion

    private void DefineContextMenuItems()
    {
        ContextMenuItems =
        [
            new () {
                Text = "Open",
                Icon = Icons.Material.Outlined.OpenInNew,
                OnClick = EventCallback.Factory.Create(this, () => OpenItem(SelectedItem))
            },
            new() {
                Text = "Create by copying",
                Icon = Icons.Material.Outlined.Add,
                OnClick = EventCallback.Factory.Create(this, CreateNewItemBasedOnExisting)
            },
        ];
    }

    private void CreateTypeClick()
    {
        TabsService.TryCreateTab<TypeCreate>();
    }

    private void OpenItem(ProductType? item)
    {
        if (item is null)
            return;

        var parameters = new Dictionary<string, object>
        {
            { nameof(TypeDetails.Id), item.Id }
        };

        TabsService.TryCreateTab<TypeDetails>(parameters);
    }

    private void OpenTypeClick(CellContext<ProductType> context)
    {
        OpenItem(context.Item);
    }

    private async Task ReloadCatalogTypes()
    {
        Items = await ProductTypeService.GetAllAsync();

        CallRequestRefresh();
    }

    private void StartedEditingItem(ProductType item)
    {
        _editTrigger = DataGridEditTrigger.Manual;
    }

    private void CanceledEditingItem(ProductType item)
    {
    }

    private async Task CommittedItemChanges(ProductType type)
    {
        await ProductTypeService.UpdateAsync(type);

        CallRequestRefresh();
    }

    private void RowClick(DataGridRowClickEventArgs<ProductType> eventArg)
    {
        SelectedItem = eventArg.Item;

        if (eventArg.MouseEventArgs.Detail is 1)
            return;

        _editMode = DataGridEditMode.Form;
        _editTrigger = DataGridEditTrigger.OnRowClick;
    }

    private async Task OnCatalogTypeChanged(ProductType changedType)
    {
        Items = await ProductTypeService.GetAllAsync();
        CallRequestRefresh();
    }

    private async Task RowClickContextMenu(DataGridRowClickEventArgs<ProductType> eventArg)
    {
        SelectedItem = eventArg.Item;
        DefineContextMenuItems();

        await ContextMenu.Show(eventArg.MouseEventArgs);
    }

    private void CreateNewItemBasedOnExisting()
    {
        if (SelectedItem is null)
            return;

        var clonedItem = SelectedItem.Clone();

        var parameters = new Dictionary<string, object>
        {
            { nameof(TypeCreate.Model), clonedItem }
        };

        TabsService.TryCreateTab<TypeCreate>(parameters);

        CallRequestRefresh();
    }

    public override void Dispose()
    {
        ProductTypeService.OnChanged -= OnCatalogTypeChanged;
    }
}
