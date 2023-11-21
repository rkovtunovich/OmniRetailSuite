using BackOffice.Application.Services.Abstraction.ProductCatalog;
using BackOffice.Client.Services;
using BackOffice.Core.Models.ProductCatalog;
using Microsoft.AspNetCore.Components.Forms;

namespace BackOffice.Client.Pages.ProductCatalog.Brand;

public partial class BrandDetails
{
    [Inject] private TabsService _tabsService { get; set; } = null!;

    [Inject] public IProductBrandService ProductBrandService { get; set; } = null!;

    [Parameter]
    public EventCallback<string> OnSaveClick { get; set; }

    [Parameter]
    public ProductBrand ProductBrand { get; set; } = null!;

    private EditContext? _editContext;

    protected override void OnInitialized()
    {
        _editContext = new EditContext(ProductBrand);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task SaveClick()
    {
        if (!_editContext?.Validate() ?? false)
            return;

        var result = await ProductBrandService.UpdateBrandAsync(ProductBrand);
        if (result is not null)
        {
            await OnSaveClick.InvokeAsync(null);
            Close();
        }
    }

    private async Task DeleteClick()
    {
        await ProductBrandService.DeleteBrandAsync(ProductBrand.Id, true);

        Close();
    }

    private void Close()
    {
        _tabsService.RemoveTab(_tabsService.Tabs?.ActivePanel);
    }
}
