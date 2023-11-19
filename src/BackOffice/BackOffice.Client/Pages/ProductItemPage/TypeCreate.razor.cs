using BackOffice.Client.Services;
using BackOffice.Core.Models.Product;
using Microsoft.AspNetCore.Components.Forms;

namespace BackOffice.Client.Pages.ProductItemPage;

public partial class TypeCreate
{
    [Inject] private TabsService _tabsService { get; set; } = null!;

    [Inject] public IProductCatalogService CatalogService { get; set; } = null!;

    [Parameter]
    public EventCallback<string> OnSaveClick { get; set; }

    private ItemType _type = new();

    private EditContext? _editContext;

    protected override void OnInitialized()
    {
        _editContext = new EditContext(_type);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task CreateClick()
    {
        if(!_editContext?.Validate() ?? false)
            return;

        var result = await CatalogItemService.CreateTypeAsync(_type);
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
