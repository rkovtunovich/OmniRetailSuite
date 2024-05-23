using BackOffice.Client.Pages.ProductCatalog.Parent;
using BackOffice.Core.Models.ProductCatalog;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using UI.Razor.Enums;

namespace BackOffice.Client.Pages.ProductCatalog.Item;

public partial class ItemList : OrsComponentBase
{
    #region Injects

    [Inject] public IProductCatalogService<ProductItem> ProductItemService { get; set; } = null!;

    [Inject] public IProductCatalogService<ProductParent> ProductParentService { get; set; } = null!;

    [Inject] private TabsService _tabsService { get; set; } = null!;

    [Inject] private IJSRuntime JSRuntime { get; set; } = null!;

    #endregion

    #region Fields

    private double _splitterPercentage = 75;

    #region Product Items

    private List<ProductItem> _productItems = [];

    private string? _searchString;

    private DataGridEditMode _editMode = DataGridEditMode.Form;
    private DataGridEditTrigger _editTrigger = DataGridEditTrigger.Manual;
    private DialogOptions _dialogOptions = new() { DisableBackdropClick = true };

    private ProductItem? _selectedItem;

    private Func<ProductItem, bool> _quickFilter => x =>
    {
        if (string.IsNullOrWhiteSpace(_searchString))
            return true;

        if (x.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        if (x.Description.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        if (x.ProductBrand?.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ?? false)
            return true;

        if (x.ProductType?.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ?? false)
            return true;

        if ($"{x.Id} {x.Price}".Contains(_searchString))
            return true;

        return false;
    };

    private string _contextMenuId = "ors-context-menu";
    private static readonly string _selectedRowClassName = "ors-selected-row";

    private MudMenu _contextMenu = null!;

    #endregion

    #region Product Parents

    private ProductParent? _selectedProductParent;

    private HashSet<ProductParent> _itemParents = [];

    private bool _isItemParentsOpen = true;

    private static readonly string _selectedParentClassName = "ors-selected-parent-item";

    #endregion

    #endregion

    #region Overrides

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            ProductItemService.OnChanged += OnProductItemChanged;
            ProductParentService.OnChanged += OnProductItemParentChanged;
            await ReloadProductItems();
            await ReloadItemParents();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    public override void Dispose()
    {
        // Unsubscribe when the component is destroyed to prevent memory leaks
        ProductItemService.OnChanged -= OnProductItemChanged;
        ProductParentService.OnChanged -= OnProductItemParentChanged;
    }

    #endregion

    #region Private Methods

    #region Product Items

    private void CreateItemClick()
    {
        _tabsService.TryCreateTab<ItemCreate>();
    }

    private void OpenItemClick(CellContext<ProductItem> context)
    {
        var parameters = new Dictionary<string, object>
        {
            { nameof(ItemDetails.Id), context.Item.Id }
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
        await ProductItemService.UpdateAsync(item);

        CallRequestRefresh();
    }

    private void RowClick(DataGridRowClickEventArgs<ProductItem> eventArg)
    {
        _selectedItem = eventArg.Item;

        if (eventArg.MouseEventArgs.Detail is 1)
            return;

        _editMode = DataGridEditMode.Form;
        _editTrigger = DataGridEditTrigger.OnRowClick;
    }

    private async Task RowClickContextMenu(DataGridRowClickEventArgs<ProductItem> eventArg)
    {
        _selectedItem = eventArg.Item;

        var contextMenu = await JSRuntime.InvokeAsync<IJSObjectReference>("document.getElementById", _contextMenuId);
        await contextMenu.InvokeVoidAsync("style.setProperty", "left", $"{eventArg.MouseEventArgs.ClientX}px");
        await contextMenu.InvokeVoidAsync("style.setProperty", "top", $"{eventArg.MouseEventArgs.ClientY}px");
        await contextMenu.InvokeVoidAsync("style.setProperty", "display", "block");

        _selectedItem = eventArg.Item;
        await _contextMenu.SetParametersAsync(ParameterView.FromDictionary(new Dictionary<string, object?>
        {
            { "Disabled", false }
        }));
        _contextMenu.OpenMenu(eventArg.MouseEventArgs);
    }

    private void CreateNewItemBasedOnExisting()
    {
        if (_selectedItem is null)
            return;

        // TODO: Create new item based on existing item

        CallRequestRefresh();
    }

    private async Task OnContextMenuOpenChanged()
    {
        if(_contextMenu.IsOpen)
            return;

        await _contextMenu.SetParametersAsync(ParameterView.FromDictionary(new Dictionary<string, object?>
        {
            { "Disabled", !_contextMenu.Disabled }
        }));
    } 

    private async Task OnProductItemChanged(ProductItem changedItem)
    {
        await ReloadProductItems();
    }

    private async Task ReloadProductItems(ProductParent? productParent = null)
    {
        if (productParent == null)
        {
            var productItemsList = await ProductItemService.GetAllAsync();
            _productItems = [.. productItemsList];
        }
        else
        {
            var productItemsList = await ProductItemService.GetByParentAsync(productParent.Id);
            _productItems = [.. productItemsList];
        }

        CallRequestRefresh();
    }

    private string SelectedRowClassFunc(ProductItem currentItem, int line)
    {
        if (_selectedItem is null)
            return string.Empty;

        if(_selectedItem.Id == currentItem.Id)
            return _selectedRowClassName;

        return string.Empty;
    }

    #endregion

    #region Product Parents

    private void ShowCatalogParentsClick()
    {
        _isItemParentsOpen = !_isItemParentsOpen;

        if (_isItemParentsOpen)
            _splitterPercentage = 75;
        else
            _splitterPercentage = 100;

        CallRequestRefresh();
    }

    private async void OnParentItemDoubleClick(ProductParent productParent, MouseEventArgs mouseEventArgs)
    {
        _selectedProductParent = productParent;
        _selectedItem = null;

        if (productParent.Name == FilterSpecialCase.All.ToString())
        {
            await ReloadProductItems();
            _selectedProductParent = null;
        }
        else
        {
            await ReloadProductItems(productParent);
        }
    }

    private void OpenItemParentClick(MudTreeViewItem<ProductParent> viewItem)
    {
        if (viewItem.Value.Name == FilterSpecialCase.Empty.ToString())
            return;

        if (viewItem.Value.Name == FilterSpecialCase.All.ToString())
            return;

        var parameters = new Dictionary<string, object>
        {
            { nameof(ItemParentDetails.Id), viewItem.Value.Id }
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
        var itemParentsList = await ProductParentService.GetAllAsync();
        _itemParents.Clear();
        _itemParents.Add(new ProductParent { Id = Guid.NewGuid(), Name = FilterSpecialCase.All.ToString() });
        _itemParents.Add(new ProductParent { Id = Guid.NewGuid(), Name = FilterSpecialCase.Empty.ToString() });

        foreach (var itemParent in itemParentsList)
        {
            _itemParents.Add(itemParent);
        }

        CallRequestRefresh();
    }

    private string GetItemClass(ProductParent productParent)
    {
        if (_selectedProductParent is null)
            return "";

        if (_selectedProductParent.Name == FilterSpecialCase.All.ToString() && productParent.Name == _selectedProductParent.Name)
            return _selectedParentClassName;

        if (_selectedProductParent.Name == FilterSpecialCase.Empty.ToString() && productParent.Name == _selectedProductParent.Name)
            return _selectedParentClassName;

        return _selectedProductParent.Id == productParent.Id ? _selectedParentClassName : "";
    }

    #endregion

    #endregion
}
