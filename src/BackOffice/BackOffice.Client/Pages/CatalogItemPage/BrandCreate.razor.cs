using BackOffice.Client.Services;
using BackOffice.Core.Models.Catalog;
using Microsoft.AspNetCore.Components.Forms;

namespace BackOffice.Client.Pages.CatalogItemPage;

public partial class BrandCreate
{
    [Inject] private TabsService _tabsService { get; set; } = null!;

    [Inject] public ICatalogService CatalogService { get; set; } = null!;

    [Parameter]
    public EventCallback<string> OnSaveClick { get; set; }

    private CatalogBrand _brand = new();

    private EditContext? _editContext;

    private List<List<int>> _forTest = [[1, 2, 3, 5, 5, 6], [1, 2, 3, 4], [2, 4, 5]];

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
        if(!_editContext?.Validate() ?? false)
            return;

        var result = await CatalogItemService.CreateBrandAsync(_brand);
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
