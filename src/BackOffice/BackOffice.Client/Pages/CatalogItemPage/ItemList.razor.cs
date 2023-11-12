using BackOffice.Application.Services.Abstraction;
using BackOffice.Client.Model;
using BackOffice.Client.Services;
using BackOffice.Core.Models.Product;

namespace BackOffice.Client.Pages.CatalogItemPage;

public partial class ItemList : BlazorComponent
{
    #region Injects

    [Inject] public IProductCatalogService CatalogService { get; set; } = null!;

    [Inject] private TabsService _tabsService { get; set; } = null!;

    #endregion

    #region Fields

    #region Item

    private List<Item> _catalogItems = new();

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

    private HashSet<TreeItemParentsData> _itemParentsViewData = new();

    private List<ItemParent> _itemParents = new();

    private bool _isItemParentsOpen = true;

    #endregion

    #endregion

    #region Overrides

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            CatalogService.CatalogItemChanged += OnCatalogItemChanged;
            await ReloadCatalogItems();
            await ReloadItemParents();;
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    public override void Dispose()
    {
        // Unsubscribe when the component is destroyed to prevent memory leaks
        CatalogService.CatalogItemChanged -= OnCatalogItemChanged;
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

    private async Task ReloadCatalogItems()
    {
        _catalogItems = await CatalogService.GetItemsAsync(0, 100);

        CallRequestRefresh();
    }

    private async Task ReloadItemParents()
    {
        _itemParents = await CatalogService.GetItemParentsAsync();
        UpdateItemParentViewData();
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
        await CatalogService.UpdateItemAsync(item);

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

    private async Task OnCatalogItemChanged(Item changedItem)
    {
        _catalogItems = await CatalogService.GetItemsAsync(0, 100, null, null);
        CallRequestRefresh();
    }

    private void UpdateItemParentViewData()
    {
        
        _itemParentsViewData = new();

        foreach(var itemParent in _itemParents)
        {
            var treeItemParentsData = TreeItemParentsData.Create(itemParent);

            _itemParentsViewData.Add(treeItemParentsData);
        }
    }

    #endregion

    #region ItemParents

    private void OpenItemParentsClick()
    {
        _isItemParentsOpen = !_isItemParentsOpen;

        CallRequestRefresh();
    }

    #endregion

    #endregion
}
