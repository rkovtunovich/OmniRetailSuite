using BackOffice.Application.Services.Abstraction.ProductCatalog;
using BackOffice.Client.Pages.ProductCatalog.Parent;
using BackOffice.Client.Services;
using BackOffice.Core.Models.ProductCatalog;

namespace BackOffice.Client.Pages.ProductCatalog.Item;

public partial class ItemList : BlazorComponent
{
    #region Injects

    [Inject] public IProductItemService ProductItemService { get; set; } = null!;

    [Inject] public IProductParentService ProductParentService { get; set; } = null!;

    [Inject] private TabsService _tabsService { get; set; } = null!;

    #endregion

    #region Fields

    double _splitterPercentage = 75;

    #region Item

    private List<ProductItem> _productItems = [];

    private MudDataGrid<ProductItem> _dataGrid = null!;

    private string? _searchString;

    private DataGridEditMode _editMode = DataGridEditMode.Form;
    private DataGridEditTrigger _editTrigger = DataGridEditTrigger.Manual;
    private DialogOptions _dialogOptions = new() { DisableBackdropClick = true };

    private Func<ProductItem, bool> _quickFilter => x =>
    {
        if (string.IsNullOrWhiteSpace(_searchString))
            return true;

        if (x.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        if (x.Description.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        if (x.Brand?.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ?? false)
            return true;

        if (x.ItemType?.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ?? false)
            return true;

        if ($"{x.Id} {x.Price}".Contains(_searchString))
            return true;

        return false;
    };

    #endregion

    #region ItemParents

    private HashSet<ProductParent> _itemParents = [];

    private bool _isItemParentsOpen = true;

    #endregion

    #endregion

    #region Overrides

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            ProductItemService.ProductItemChanged += OnProductItemChanged;
            await ReloadProductItems();
            await ReloadItemParents(); ;
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    public override void Dispose()
    {
        // Unsubscribe when the component is destroyed to prevent memory leaks
        ProductItemService.ProductItemChanged -= OnProductItemChanged;
    }

    #endregion

    #region Private Methods

    #region Item

    private void CreateItemClick()
    {
        _tabsService.TryCreateTab<ItemCreate>();
    }

    private void OpenItemClick(CellContext<ProductItem> context)
    {
        var parameters = new Dictionary<string, object>
        {
            { nameof(ItemDetails.ProductItem), context.Item }
        };

        _tabsService.TryCreateTab<ItemDetails>(parameters);
    }

    private void StartedEditingItem(ProductItem item)
    {
        _editTrigger = DataGridEditTrigger.Manual;
    }

    private void CanceledEditingItem(ProductItem item)
    {
    }

    private async Task CommittedItemChanges(ProductItem item)
    {
        await ProductItemService.UpdateItemAsync(item);

        CallRequestRefresh();
    }

    private async Task RowClick(DataGridRowClickEventArgs<ProductItem> eventArg)
    {
        if (eventArg.MouseEventArgs.Detail == 1)
            return;

        _editMode = DataGridEditMode.Form;
        _editTrigger = DataGridEditTrigger.OnRowClick;

        await Task.Run(() =>
        {
        });
    }

    private async Task OnProductItemChanged(ProductItem changedItem)
    {
        await ReloadProductItems();
    }

    private async Task ReloadProductItems()
    {
        _productItems = await ProductItemService.GetItemsAsync(0, 100);

        CallRequestRefresh();
    }

    #endregion

    #region ItemParents

    private void ShowCatalogParentsClick()
    {
        _isItemParentsOpen = !_isItemParentsOpen;

        if(_isItemParentsOpen)
            _splitterPercentage = 75;
        else
            _splitterPercentage = 100;

        CallRequestRefresh();
    }

    private void OpenItemParentClick(MudTreeViewItem<ProductParent> viewItem)
    {
        var parameters = new Dictionary<string, object>
        {
            { nameof(ItemParentDetails.ProductParent), viewItem.Value }
        };

        _tabsService.TryCreateTab<ItemParentDetails>(parameters);
    }

    private void CreateCatalogParentClick()
    {
        _tabsService.TryCreateTab<ItemParentCreate>();
    }

    private async Task OnProductItemParentChanged(ProductParent changedItem)
    {
        await ReloadItemParents();
    }

    private async Task ReloadItemParents()
    {
        var itemParentsList = await ProductParentService.GetItemParentsAsync();
        _itemParents = [.. itemParentsList];
        CallRequestRefresh();
    }

    #endregion

    #endregion
}
