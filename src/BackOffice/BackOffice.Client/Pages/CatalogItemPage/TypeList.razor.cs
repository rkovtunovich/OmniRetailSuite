using BackOffice.Client.Services;
using BackOffice.Core.Models.Catalog;

namespace BackOffice.Client.Pages.CatalogItemPage;

public partial class TypeList : BlazorComponent
{
    [Inject] public ICatalogService CatalogService { get; set; } = null!;

    [Inject] private TabsService _tabsService { get; set; } = null!;

    private List<CatalogType> _catalogTypes = new();

    private MudDataGrid<CatalogType> _dataGrid = null!;

    private string? _searchString;

    private DataGridEditMode _editMode = DataGridEditMode.Form;
    private DataGridEditTrigger _editTrigger = DataGridEditTrigger.Manual;
    private DialogOptions _dialogOptions = new() { DisableBackdropClick = true };

    private Func<CatalogType, bool> _quickFilter => x =>
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

    private void OpenTypeClick(CellContext<CatalogType> context)
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

    private void StartedEditingItem(CatalogType item)
    {
        _editTrigger = DataGridEditTrigger.Manual;
    }

    private void CanceledEditingItem(CatalogType item)
    {
    }

    private async Task CommittedItemChanges(CatalogType type)
    {
        await CatalogService.UpdateTypeAsync(type);

        CallRequestRefresh();
    }

    private async Task RowClick(DataGridRowClickEventArgs<CatalogType> eventArg)
    {
        if (eventArg.MouseEventArgs.Detail == 1)
            return;

        _editMode = DataGridEditMode.Form;
        _editTrigger = DataGridEditTrigger.OnRowClick;

        await Task.Run(() =>
        {
        });
    }

    private async Task OnCatalogTypeChanged(CatalogType changedType)
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
