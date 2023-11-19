using BackOffice.Client.Services;
using BackOffice.Core.Models.Product;

namespace BackOffice.Client.Pages.ProductItemPage;

public partial class TypeList : BlazorComponent
{
    [Inject] public IProductCatalogService CatalogService { get; set; } = null!;

    [Inject] private TabsService _tabsService { get; set; } = null!;

    private List<ItemType> _catalogTypes = new();

    private MudDataGrid<ItemType> _dataGrid = null!;

    private string? _searchString;

    private DataGridEditMode _editMode = DataGridEditMode.Form;
    private DataGridEditTrigger _editTrigger = DataGridEditTrigger.Manual;
    private DialogOptions _dialogOptions = new() { DisableBackdropClick = true };

    private Func<ItemType, bool> _quickFilter => x =>
    {
        if (string.IsNullOrWhiteSpace(_searchString))
            return true;

        if (x.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    };

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            CatalogService.CatalogTypeChanged += OnCatalogTypeChanged;

            await ReloadCatalogTypes();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private void CreateTypeClick()
    {
        _tabsService.TryCreateTab<TypeCreate>();        
    }

    private void OpenTypeClick(CellContext<ItemType> context)
    {
        var parameters = new Dictionary<string, object>
        {
            { nameof(TypeDetails.Type), context.Item }
        };

        _tabsService.TryCreateTab<TypeDetails>(parameters);
    }

    private async Task ReloadCatalogTypes()
    {
        _catalogTypes = await CatalogService.GetTypesAsync();

        CallRequestRefresh();
    }

    private void StartedEditingItem(ItemType item)
    {
        _editTrigger = DataGridEditTrigger.Manual;
    }

    private void CanceledEditingItem(ItemType item)
    {
    }

    private async Task CommittedItemChanges(ItemType type)
    {
        await CatalogService.UpdateTypeAsync(type);

        CallRequestRefresh();
    }

    private async Task RowClick(DataGridRowClickEventArgs<ItemType> eventArg)
    {
        if (eventArg.MouseEventArgs.Detail == 1)
            return;

        _editMode = DataGridEditMode.Form;
        _editTrigger = DataGridEditTrigger.OnRowClick;

        await Task.Run(() =>
        {
        });
    }

    private async Task OnCatalogTypeChanged(ItemType changedType)
    {
        _catalogTypes = await CatalogService.GetTypesAsync();
        CallRequestRefresh();
    }

    public override void Dispose()
    {
        // Unsubscribe when the component is destroyed to prevent memory leaks
        CatalogService.CatalogTypeChanged -= OnCatalogTypeChanged;
    }
}
