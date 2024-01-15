using BackOffice.Application.Services.Abstraction.ProductCatalog;
using BackOffice.Client.Components.Base;
using BackOffice.Client.Services;
using BackOffice.Core.Models.ProductCatalog;

namespace BackOffice.Client.Pages.ProductCatalog.Brand;

public partial class BrandList : OrsComponentBase
{
    [Inject] public IProductBrandService CatalogService { get; set; } = null!;

    [Inject] private TabsService _tabsService { get; set; } = null!;

    private List<ProductBrand> _catalogBrands = [];

    private string? _searchString;

    private DataGridEditMode _editMode = DataGridEditMode.Form;
    private DataGridEditTrigger _editTrigger = DataGridEditTrigger.Manual;
    private DialogOptions _dialogOptions = new() { DisableBackdropClick = true };

    private Func<ProductBrand, bool> _quickFilter => x =>
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
            CatalogService.ProductBrandChanged += OnCatalogBrandChanged;

            await ReloadCatalogTypes();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private void CreateBrandClick()
    {
        _tabsService.TryCreateTab<BrandCreate>();
    }

    private void OpenBrandClick(CellContext<ProductBrand> context)
    {
        var parameters = new Dictionary<string, object>
        {
            { nameof(BrandDetails.Model), context.Item }
        };

        _tabsService.TryCreateTab<BrandDetails>(parameters);
    }

    private async Task ReloadCatalogTypes()
    {
        _catalogBrands = await CatalogService.GetBrandsAsync();

        CallRequestRefresh();
    }

    private void StartedEditingItem(ProductBrand item)
    {
        _editTrigger = DataGridEditTrigger.Manual;
    }

    private void CanceledEditingItem(ProductBrand item)
    {
    }

    private async Task CommittedItemChanges(ProductBrand type)
    {
        await CatalogService.UpdateBrandAsync(type);

        CallRequestRefresh();
    }

    private async Task RowClick(DataGridRowClickEventArgs<ProductBrand> eventArg)
    {
        if (eventArg.MouseEventArgs.Detail == 1)
            return;

        _editMode = DataGridEditMode.Form;
        _editTrigger = DataGridEditTrigger.OnRowClick;

        await Task.Run(() =>
        {
        });
    }

    private async Task OnCatalogBrandChanged(ProductBrand changedBrand)
    {
        _catalogBrands = await CatalogService.GetBrandsAsync();
        CallRequestRefresh();
    }

    public override void Dispose()
    {
        // Unsubscribe when the component is destroyed to prevent memory leaks
        CatalogService.ProductBrandChanged -= OnCatalogBrandChanged;
    }
}
