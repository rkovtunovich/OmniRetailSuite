using BackOffice.Core.Models.ProductCatalog;
using Microsoft.JSInterop;

namespace BackOffice.Client.Pages.ProductCatalog.Brand;

public partial class BrandList : ListBase<ProductBrand>
{
    [Inject] public IProductCatalogService<ProductBrand> ProductBrandService { get; set; } = null!;

    [Inject] private IJSRuntime JSRuntime { get; set; } = null!;

    private DataGridEditMode _editMode = DataGridEditMode.Form;
    private DataGridEditTrigger _editTrigger = DataGridEditTrigger.Manual;
    private DialogOptions _dialogOptions = new() { DisableBackdropClick = true };

    private string _contextMenuId = "ors-context-menu";
    private MudMenu _contextMenu = null!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            ProductBrandService.OnChanged += OnCatalogBrandChanged;

            await ReloadCatalogTypes();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private void CreateBrandClick()
    {
        TabsService.TryCreateTab<BrandCreate>();
    }

    private void OpenBrandClick(CellContext<ProductBrand> context)
    {
        var parameters = new Dictionary<string, object>
        {
            { nameof(BrandDetails.Id), context.Item.Id }
        };

        TabsService.TryCreateTab<BrandDetails>(parameters);
    }

    private async Task ReloadCatalogTypes()
    {
        Items = await ProductBrandService.GetAllAsync();

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
        await ProductBrandService.UpdateAsync(type);

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
        Items = await ProductBrandService.GetAllAsync();
        CallRequestRefresh();
    }

    private async Task RowClickContextMenu(DataGridRowClickEventArgs<ProductBrand> eventArg)
    {
        SelectedItem = eventArg.Item;

        var contextMenu = await JSRuntime.InvokeAsync<IJSObjectReference>("document.getElementById", _contextMenuId);
        await contextMenu.InvokeVoidAsync("style.setProperty", "left", $"{eventArg.MouseEventArgs.ClientX}px");
        await contextMenu.InvokeVoidAsync("style.setProperty", "top", $"{eventArg.MouseEventArgs.ClientY}px");
        await contextMenu.InvokeVoidAsync("style.setProperty", "display", "block");

        SelectedItem = eventArg.Item;
        await _contextMenu.SetParametersAsync(ParameterView.FromDictionary(new Dictionary<string, object?>
        {
            { "Disabled", false }
        }));
        _contextMenu.OpenMenu(eventArg.MouseEventArgs);
    }

    public override void Dispose()
    {
        // Unsubscribe when the component is destroyed to prevent memory leaks
        ProductBrandService.OnChanged -= OnCatalogBrandChanged;
    }
}
