using BackOffice.Core.Models.ProductCatalog;

namespace BackOffice.Client.Components.Pages.ProductCatalog.BrandPages;

public partial class BrandList : ListBase<ProductBrand>
{
    [Inject] public IProductCatalogService<ProductBrand> ProductBrandService { get; set; } = null!;

    [Inject] private IStringLocalizer<BrandList> _localizer { get; set; } = default!;

    private DialogOptions _dialogOptions = new() { DisableBackdropClick = true };

    #region Overrides

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            ProductBrandService.OnChanged += OnCatalogBrandChanged;

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

    private void CreateBrandClick()
    {
        TabsService.TryCreateTab<BrandCreate>();
    }

    private void OpenItem(ProductBrand? item)
    {
        if (item is null)
            return;

        var parameters = new Dictionary<string, object>
        {
            { nameof(BrandDetails.Id), item.Id }
        };

        TabsService.TryCreateTab<BrandDetails>(parameters);
    }

    private async Task ReloadCatalogTypes()
    {
        Items = await ProductBrandService.GetAllAsync();

        CallRequestRefresh();
    }

    private void RowClick(DataGridRowClickEventArgs<ProductBrand> eventArg)
    {
        SelectedItem = eventArg.Item;

        if (eventArg.MouseEventArgs.Detail is 1)
            return;

        OpenItem(SelectedItem);
    }

    private async Task OnCatalogBrandChanged(ProductBrand changedBrand)
    {
        Items = await ProductBrandService.GetAllAsync();
        CallRequestRefresh();
    }

    private async Task RowClickContextMenu(DataGridRowClickEventArgs<ProductBrand> eventArg)
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
            { nameof(BrandCreate.Model), clonedItem }
        };

        TabsService.TryCreateTab<BrandCreate>(parameters);

        CallRequestRefresh();
    }

    public override void Dispose()
    {
        // Unsubscribe when the component is destroyed to prevent memory leaks
        ProductBrandService.OnChanged -= OnCatalogBrandChanged;
    }
}
