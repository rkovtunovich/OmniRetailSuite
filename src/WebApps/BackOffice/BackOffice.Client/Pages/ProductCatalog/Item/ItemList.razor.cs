using BackOffice.Client.Pages.ProductCatalog.Parent;
using BackOffice.Core.Models.ProductCatalog;
using Microsoft.AspNetCore.Components.Web;
using UI.Razor.Enums;

namespace BackOffice.Client.Pages.ProductCatalog.Item;

public partial class ItemList : ListBase<ProductItem>
{
    #region Injects

    [Inject] public IProductCatalogService<ProductItem> ProductItemService { get; set; } = null!;

    [Inject] public IProductCatalogService<ProductParent> ProductParentService { get; set; } = null!;

    #endregion

    #region Fields

    private double _splitterPercentage = 75;

    #region Product Items

    private DataGridEditMode _editMode = DataGridEditMode.Form;
    private DataGridEditTrigger _editTrigger = DataGridEditTrigger.Manual;
    private DialogOptions _dialogOptions = new() { DisableBackdropClick = true };

    #endregion

    #region Product Parents

    private ProductParent? _selectedProductParent;

    private HashSet<ProductParent> _itemParents = [];

    private bool _isItemParentsOpen = true;

    private static readonly string _selectedParentClassName = "ors-selected-parent-item";

    #endregion

    #endregion

    #region Overrides

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
        TabsService.TryCreateTab<ItemCreate>();
    }

    private void OpenItemClick(CellContext<ProductItem> context)
    {
        OpenItem(context.Item);
    }

    private void OpenItem(ProductItem? item)
    {
        if (item is null)
            return;

        var parameters = new Dictionary<string, object>
        {
            { nameof(ItemDetails.Id), item.Id }
        };

        TabsService.TryCreateTab<ItemDetails>(parameters);
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
        SelectedItem = eventArg.Item;

        if (eventArg.MouseEventArgs.Detail is 1)
            return;

        _editMode = DataGridEditMode.Form;
        _editTrigger = DataGridEditTrigger.OnRowClick;
    }

    private async Task RowClickContextMenu(DataGridRowClickEventArgs<ProductItem> eventArg)
    {
        SelectedItem = eventArg.Item;
        DefineContextMenuItems();

        await ContextMenu.ShowContextMenu(eventArg.MouseEventArgs);
    }

    private void CreateNewItemBasedOnExisting()
    {
        if (SelectedItem is null)
            return;

        var clonedItem = SelectedItem.Clone();

        var parameters = new Dictionary<string, object>
        {
            { nameof(ItemCreate.Model), clonedItem }
        };

        TabsService.TryCreateTab<ItemCreate>(parameters);

        CallRequestRefresh();
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
            Items = [.. productItemsList];
        }
        else
        {
            var productItemsList = await ProductItemService.GetByParentAsync(productParent.Id);
            Items = [.. productItemsList];
        }

        CallRequestRefresh();
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
        SelectedItem = null;

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

        TabsService.TryCreateTab<ItemParentDetails>(parameters);
    }

    private void CreateCatalogParentClick()
    {
        TabsService.TryCreateTab<ItemParentCreate>();
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
