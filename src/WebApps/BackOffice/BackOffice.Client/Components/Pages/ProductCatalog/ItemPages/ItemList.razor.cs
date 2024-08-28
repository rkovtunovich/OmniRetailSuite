using BackOffice.Client.Components.Pages.ProductCatalog.ParentPages;
using BackOffice.Core.Models.ProductCatalog;
using UI.Razor.Enums;

namespace BackOffice.Client.Components.Pages.ProductCatalog.ItemPages;

public partial class ItemList : ListBase<ProductItem>
{
    #region Injects

    [Inject] public IProductCatalogService<ProductItem> ProductItemService { get; set; } = null!;

    [Inject] public IProductCatalogService<ProductParent> ProductParentService { get; set; } = null!;

    [Inject] private IStringLocalizer<ItemList> _localizer { get; set; } = default!;

    #endregion

    #region Fields

    private double _splitterPercentage = 75;

    #region Product Items

    private DialogOptions _dialogOptions = new() { BackdropClick = false };

    #endregion

    #region Product Parents

    private ProductParent? _selectedProductParent;

    private IReadOnlyCollection<TreeItemData<ProductParent>> _itemParents = [];

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

    private void DefineContextMenuItems()
    {
        ContextMenuItems =
        [
            new () {
                Text = @_localizer["Open"],
                Icon = Icons.Material.Outlined.OpenInNew,
                OnClick = EventCallback.Factory.Create(this, () => OpenItem(SelectedItem))
            },
            new() {
                Text = @_localizer["CreateByCopying"],
                Icon = Icons.Material.Outlined.Add,
                OnClick = EventCallback.Factory.Create(this, CreateNewItemBasedOnExisting)
            },
        ];
    }

    private void CreateItemClick()
    {
        TabsService.TryCreateTab<ItemCreate>();
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

    private void RowClick(DataGridRowClickEventArgs<ProductItem> eventArg)
    {
        SelectedItem = eventArg.Item;

        if (eventArg.MouseEventArgs.Detail is 1)
            return;

        OpenItem(SelectedItem);
    }

    private async Task RowClickContextMenu(DataGridRowClickEventArgs<ProductItem> eventArg)
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

    private async void OnParentItemDoubleClick(TreeItemData<ProductParent> treeItemData)
    {
        var productParent = treeItemData.Value;

        _selectedProductParent = productParent;
        SelectedItem = null;

        if (productParent?.Name == FilterSpecialCase.All.ToString())
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
        if (viewItem.Value?.Name == FilterSpecialCase.Empty.ToString())
            return;

        if (viewItem.Value?.Name == FilterSpecialCase.All.ToString())
            return;

        if (viewItem.Value is null)
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
        var itemParents = new List<ProductParent>
        {
            new() { Id = Guid.NewGuid(), Name = FilterSpecialCase.All.ToString() },
            new() { Id = Guid.NewGuid(), Name = FilterSpecialCase.Empty.ToString() }
        };
        itemParents.AddRange(await ProductParentService.GetAllAsync());

       _itemParents = CreateTreeItems(itemParents);

        CallRequestRefresh();
    }

    private string GetItemClass(TreeItemData<ProductParent> viewItem)
    {
        if (_selectedProductParent is null)
            return "";

        var productParent = viewItem.Value;

        if (_selectedProductParent.Name == FilterSpecialCase.All.ToString() && productParent?.Name == _selectedProductParent.Name)
            return _selectedParentClassName;

        if (_selectedProductParent.Name == FilterSpecialCase.Empty.ToString() && productParent?.Name == _selectedProductParent.Name)
            return _selectedParentClassName;

        return _selectedProductParent.Id == productParent?.Id ? _selectedParentClassName : "";
    }

    private static List<TreeItemData<ProductParent>> CreateTreeItems(List<ProductParent>? items)
    {
        if (items is null)
            return [];

        var treeItems = new List<TreeItemData<ProductParent>>();

        foreach (var item in items)
        {
            var treeItem = new TreeItemData<ProductParent>
            {
                Value = item,
                Children = CreateTreeItems(item.Children)
            };

            treeItems.Add(treeItem);
        }

        return treeItems;
    }

    #endregion

    #endregion
}
