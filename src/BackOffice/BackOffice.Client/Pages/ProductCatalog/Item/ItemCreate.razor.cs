using BackOffice.Application.Services.Abstraction.ProductCatalog;
using BackOffice.Client.Services;
using BackOffice.Core.Models.ProductCatalog;
using Microsoft.AspNetCore.Components.Forms;

namespace BackOffice.Client.Pages.ProductCatalog.Item;

public partial class ItemCreate
{
    [Inject] private TabsService _tabsService { get; set; } = null!;

    [Inject] public IProductItemService ProductItemService { get; set; } = null!;

    [Inject] public IProductBrandService ProductBrandService { get; set; } = null!;

    [Inject] public IProductTypeService ProductTypeService { get; set; } = null!;

    [Parameter]
    public EventCallback<string> OnSaveClick { get; set; }

    private List<ProductType> _itemTypes = [];

    private List<ProductBrand> _itemBrands = [];

    private string LoadPicture => string.IsNullOrEmpty(_item.PictureBase64) ? string.Empty : $"data:image/png;base64, {_item.PictureBase64}";

    private bool HasPicture => !string.IsNullOrEmpty(_item.PictureBase64);

    private string _badFileMessage = string.Empty;

    private ProductItem _item = new();

    private EditContext? _editContext;

    protected override void OnInitialized()
    {
        _editContext = new EditContext(_item);
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

    private async Task CreateClick()
    {
        if (!_editContext?.Validate() ?? false)
            return;

        var result = await ProductItemService.CreateItemAsync(_item);
        if (result is not null)
        {
            await OnSaveClick.InvokeAsync(null);
            Close();
        }
    }

    private void Close()
    {
        _tabsService.RemoveTab(_tabsService.Tabs?.ActivePanel);
    }

    private Func<ProductType, string> _converterItemType = p => p?.Name ?? "";

    private Func<ProductBrand, string> _converterItemBrand = p => p?.Name ?? "";
}
