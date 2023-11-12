using BackOffice.Application.Services.Abstraction;
using BackOffice.Client.Services;
using BackOffice.Core.Models.Product;

namespace BackOffice.Client.Pages.CatalogItemPage;

public partial class BrandList : BlazorComponent
{
    [Inject] public IProductCatalogService CatalogService { get; set; } = null!;

    [Inject] private TabsService _tabsService { get; set; } = null!;

    private List<Brand> _catalogBrands = new();

    private MudDataGrid<Brand> _dataGrid = null!;

    private string? _searchString;

    private DataGridEditMode _editMode = DataGridEditMode.Form;
    private DataGridEditTrigger _editTrigger = DataGridEditTrigger.Manual;
    private DialogOptions _dialogOptions = new() { DisableBackdropClick = true };

    private Func<Brand, bool> _quickFilter => x =>
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
            CatalogService.CatalogBrandChanged += OnCatalogBrandChanged;

            await ReloadCatalogTypes();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private void CreateBrandClick()
    {
        _tabsService.TryCreateTab<BrandCreate>();        
    }

    private void OpenBrandClick(CellContext<Brand> context)
    {
        var parameters = new Dictionary<string, object>
        {
            { nameof(BrandDetails.Brand), context.Item }
        };

        _tabsService.TryCreateTab<BrandDetails>(parameters);
    }

    private async Task ReloadCatalogTypes()
    {
        _catalogBrands = await CatalogService.GetBrandsAsync();

        CallRequestRefresh();
    }

    private void StartedEditingItem(Brand item)
    {
        _editTrigger = DataGridEditTrigger.Manual;
    }

    private void CanceledEditingItem(Brand item)
    {
    }

    private async Task CommittedItemChanges(Brand type)
    {
        await CatalogService.UpdateBrandAsync(type);

        CallRequestRefresh();
    }

    private async Task RowClick(DataGridRowClickEventArgs<Brand> eventArg)
    {
        if (eventArg.MouseEventArgs.Detail == 1)
            return;

        _editMode = DataGridEditMode.Form;
        _editTrigger = DataGridEditTrigger.OnRowClick;

        await Task.Run(() =>
        {
        });
    }

    private async Task OnCatalogBrandChanged(Brand changedBrand)
    {
        _catalogBrands = await CatalogService.GetBrandsAsync();
        CallRequestRefresh();
    }

    public override void Dispose()
    {
        // Unsubscribe when the component is destroyed to prevent memory leaks
        CatalogService.CatalogBrandChanged -= OnCatalogBrandChanged;
    }
}
