using BackOffice.Application.Services.Abstraction.ProductCatalog;
using BackOffice.Client.Services;
using BackOffice.Core.Models.ProductCatalog;
using Microsoft.AspNetCore.Components.Forms;

namespace BackOffice.Client.Pages.ProductCatalog.Brand;

public partial class BrandCreate
{
    [Inject] private TabsService _tabsService { get; set; } = null!;

    [Inject] public IProductBrandService ProductBrandService { get; set; } = null!;

    [Parameter]
    public EventCallback<string> OnSaveClick { get; set; }

    private ProductBrand _brand = new();

    private EditContext? _editContext;

    protected override void OnInitialized()
    {
        _editContext = new EditContext(_brand);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task CreateClick()
    {
        if (!_editContext?.Validate() ?? false)
            return;

        var result = await ProductBrandService.CreateBrandAsync(_brand);
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
}
