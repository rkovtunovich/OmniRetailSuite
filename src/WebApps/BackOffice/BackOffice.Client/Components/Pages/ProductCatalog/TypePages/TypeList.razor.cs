using BackOffice.Core.Models.ProductCatalog;

namespace BackOffice.Client.Components.Pages.ProductCatalog.TypePages;

public partial class TypeList : ListBase<ProductType>
{
    [Inject] public IProductCatalogService<ProductType> ProductTypeService { get; set; } = null!;

    [Inject] private IStringLocalizer<TypeList> _localizer { get; set; } = default!;

    private DialogOptions _dialogOptions = new() { BackdropClick = false };

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
                Text = _localizer["Open"],
                Icon = Icons.Material.Outlined.OpenInNew,
                OnClick = EventCallback.Factory.Create(this, () => OpenItem(SelectedItem))
            },
            new() {
                Text = _localizer["CreateByCopying"],
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

    private async Task ReloadCatalogTypes()
    {
        Items = await ProductTypeService.GetAllAsync();

        CallRequestRefresh();
    }


    private void RowClick(DataGridRowClickEventArgs<ProductType> eventArg)
    {
        SelectedItem = eventArg.Item;

        if (eventArg.MouseEventArgs.Detail is 1)
            return;

        OpenItem(SelectedItem);
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
