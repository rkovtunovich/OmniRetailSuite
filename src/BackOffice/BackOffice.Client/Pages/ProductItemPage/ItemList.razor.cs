using BackOffice.Client.Services;
using BackOffice.Core.Models.Product;

namespace BackOffice.Client.Pages.ProductItemPage;

public partial class ItemList : BlazorComponent
{
    #region Injects

    [Inject] public IProductCatalogService ProductService { get; set; } = null!;

    [Inject] private TabsService _tabsService { get; set; } = null!;

    #endregion

    #region Fields

    #region Item

    private List<Item> _productItems = new();

    private MudDataGrid<Item> _dataGrid = null!;

    private string? _searchString;

    private DataGridEditMode _editMode = DataGridEditMode.Form;
    private DataGridEditTrigger _editTrigger = DataGridEditTrigger.Manual;
    private DialogOptions _dialogOptions = new() { DisableBackdropClick = true };

    private Func<Item, bool> _quickFilter => x =>
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

    private HashSet<ItemParent> _itemParents = [];

    private bool _isItemParentsOpen = true;

    #endregion

    #endregion

    #region Overrides

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            ProductService.CatalogItemChanged += OnProductItemChanged;
            await ReloadProductItems();
            await ReloadItemParents();;
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    public override void Dispose()
    {
        // Unsubscribe when the component is destroyed to prevent memory leaks
        ProductService.CatalogItemChanged -= OnProductItemChanged;
    }

    #endregion

    #region Private Methods

    #region Item

    private void CreateItemClick()
    {
        _tabsService.TryCreateTab<ItemCreate>();        
    }

    private void OpenItemClick(CellContext<Item> context)
    {
        var parameters = new Dictionary<string, object>
        {
            { nameof(ItemDetails.Item), context.Item }
        };

        _tabsService.TryCreateTab<ItemDetails>(parameters);
    }

    private async Task ReloadProductItems()
    {
        _productItems = await ProductService.GetItemsAsync(0, 100);

        CallRequestRefresh();
    }

    private async Task ReloadItemParents()
    {
        var itemParentsList = await ProductService.GetItemParentsAsync();
        _itemParents = [.. itemParentsList];
        CallRequestRefresh();
    }

    private void StartedEditingItem(Item item)
    {
        _editTrigger = DataGridEditTrigger.Manual;
    }

    private void CanceledEditingItem(Item item)
    {
    }

    private async Task CommittedItemChanges(Item item)
    {
        await ProductService.UpdateItemAsync(item);

        CallRequestRefresh();
    }

    private async Task RowClick(DataGridRowClickEventArgs<Item> eventArg)
    {
        if (eventArg.MouseEventArgs.Detail == 1)
            return;

        _editMode = DataGridEditMode.Form;
        _editTrigger = DataGridEditTrigger.OnRowClick;

        await Task.Run(() =>
        {
        });
    }

    private async Task OnProductItemChanged(Item changedItem)
    {
        _productItems = await ProductService.GetItemsAsync(0, 100, null, null);
        CallRequestRefresh();
    }

    #endregion

    #region ItemParents

    private void OpenItemParentsClick()
    {
        _isItemParentsOpen = !_isItemParentsOpen;

        CallRequestRefresh();
    }

    private void OpenItemParentClick(MudTreeViewItem<ItemParent> viewItem)
    {
        var parameters = new Dictionary<string, object>
        {
            { nameof(ItemParentDetails.ItemParent), viewItem.Value }
        };

        _tabsService.TryCreateTab<ItemParentDetails>(parameters);
    }

    private void CreateItemParentClick()
    {
        _tabsService.TryCreateTab<ItemParentCreate>();
    }

    #endregion

    #endregion
}
