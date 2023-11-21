using BackOffice.Application.Services.Abstraction.ProductCatalog;
using BackOffice.Client.Services;
using BackOffice.Core.Models.ProductCatalog;
using Microsoft.AspNetCore.Components.Forms;

namespace BackOffice.Client.Pages.ProductCatalog.Item;

public partial class ItemDetails
{
    [Inject] private TabsService _tabsService { get; set; } = null!;

    [Inject] public IProductItemService ProductItemService { get; set; } = null!;

    [Inject] public IProductTypeService ProductTypeService { get; set; } = null!;

    [Inject] public IProductBrandService ProductBrandService { get; set; } = null!;

    [Parameter]
    public EventCallback<string> OnSaveClick { get; set; }

    [Parameter]
    public ProductItem ProductItem { get; set; } = null!;

    private List<ProductType> _itemTypes = new();

    private List<ProductBrand> _itemBrands = new();

    private string LoadPicture => string.IsNullOrEmpty(ProductItem.PictureBase64) ? string.Empty : $"data:image/png;base64, {ProductItem.PictureBase64}";

    private bool HasPicture => !string.IsNullOrEmpty(ProductItem.PictureBase64);

    private string _badFileMessage = string.Empty;

    private EditContext? _editContext;

    protected override void OnInitialized()
    {
        _editContext = new EditContext(ProductItem);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _itemTypes = await ProductTypeService.GetTypesAsync();
            _itemBrands = await ProductBrandService.GetBrandsAsync();

            CallRequestRefresh();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task SaveClick()
    {
        if (!_editContext?.Validate() ?? false)
            return;

        var result = await ProductItemService.UpdateItemAsync(ProductItem);
        if (result is not null)
        {
            await OnSaveClick.InvokeAsync(null);
            Close();
        }
    }

    private async Task DeleteClick()
    {
        await ProductItemService.DeleteItemAsync(ProductItem.Id, true);

        Close();
    }

    private void Close()
    {
        _tabsService.RemoveTab(_tabsService.Tabs?.ActivePanel);
    }

    private Func<ProductType, string> _converterItemType = p => p?.Name ?? "";

    private Func<ProductBrand, string> _converterItemBrand = p => p?.Name ?? "";
}
