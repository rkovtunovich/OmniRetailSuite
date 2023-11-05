using BackOffice.Client.Services;
using BackOffice.Core.Models.Catalog;
using Microsoft.AspNetCore.Components.Forms;

namespace BackOffice.Client.Pages.CatalogItemPage;

public partial class ItemCreate
{
    [Inject] private TabsService _tabsService { get; set; } = null!;

    [Inject] public ICatalogService CatalogService { get; set; } = null!;

    [Parameter]
    public EventCallback<string> OnSaveClick { get; set; }

    private List<CatalogType> _itemTypes = new();

    private List<CatalogBrand> _itemBrands = new();

    private string LoadPicture => string.IsNullOrEmpty(_item.PictureBase64) ? string.Empty : $"data:image/png;base64, {_item.PictureBase64}";

    private bool HasPicture => !string.IsNullOrEmpty(_item.PictureBase64);

    private string _badFileMessage = string.Empty;

    private CatalogItem _item = new();

    private EditContext? _editContext;

    protected override void OnInitialized()
    {
        _editContext = new EditContext(_item);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _itemTypes = await CatalogService.GetTypesAsync();
            _itemBrands = await CatalogService.GetBrandsAsync();

            CallRequestRefresh();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task CreateClick()
    {
        if(!_editContext?.Validate() ?? false)
            return;

        var result = await CatalogItemService.CreateItemAsync(_item);
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

    private Func<CatalogType, string> _converterItemType = p => p?.Name ?? "";

    private Func<CatalogBrand, string> _converterItemBrand = p => p?.Name ?? "";
}
