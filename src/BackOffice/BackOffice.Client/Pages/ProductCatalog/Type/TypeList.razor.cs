using BackOffice.Application.Services.Abstraction.ProductCatalog;
using BackOffice.Client.Components.Base;
using BackOffice.Client.Services;
using BackOffice.Core.Models.ProductCatalog;

namespace BackOffice.Client.Pages.ProductCatalog.Type;

public partial class TypeList : OrsComponentBase
{
    [Inject] public IProductTypeService ProductTypeService { get; set; } = null!;

    [Inject] private TabsService _tabsService { get; set; } = null!;

    private List<ProductType> _catalogTypes = new();

    private MudDataGrid<ProductType> _dataGrid = null!;

    private string? _searchString;

    private DataGridEditMode _editMode = DataGridEditMode.Form;
    private DataGridEditTrigger _editTrigger = DataGridEditTrigger.Manual;
    private DialogOptions _dialogOptions = new() { DisableBackdropClick = true };

    private Func<ProductType, bool> _quickFilter => x =>
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
            ProductTypeService.ProductTypeChanged += OnCatalogTypeChanged;

            await ReloadCatalogTypes();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private void CreateTypeClick()
    {
        _tabsService.TryCreateTab<TypeCreate>();
    }

    private void OpenTypeClick(CellContext<ProductType> context)
    {
        var parameters = new Dictionary<string, object>
        {
            { nameof(TypeDetails.Type), context.Item }
        };

        _tabsService.TryCreateTab<TypeDetails>(parameters);
    }

    private async Task ReloadCatalogTypes()
    {
        _catalogTypes = await ProductTypeService.GetTypesAsync();

        CallRequestRefresh();
    }

    private void StartedEditingItem(ProductType item)
    {
        _editTrigger = DataGridEditTrigger.Manual;
    }

    private void CanceledEditingItem(ProductType item)
    {
    }

    private async Task CommittedItemChanges(ProductType type)
    {
        await ProductTypeService.UpdateTypeAsync(type);

        CallRequestRefresh();
    }

    private async Task RowClick(DataGridRowClickEventArgs<ProductType> eventArg)
    {
        if (eventArg.MouseEventArgs.Detail == 1)
            return;

        _editMode = DataGridEditMode.Form;
        _editTrigger = DataGridEditTrigger.OnRowClick;

        await Task.Run(() =>
        {
        });
    }

    private async Task OnCatalogTypeChanged(ProductType changedType)
    {
        _catalogTypes = await ProductTypeService.GetTypesAsync();
        CallRequestRefresh();
    }

    public override void Dispose()
    {
        // Unsubscribe when the component is destroyed to prevent memory leaks
        ProductTypeService.ProductTypeChanged -= OnCatalogTypeChanged;
    }
}
