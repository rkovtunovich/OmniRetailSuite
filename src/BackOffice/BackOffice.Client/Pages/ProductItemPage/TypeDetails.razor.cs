using BackOffice.Client.Services;
using BackOffice.Core.Models.Product;
using Microsoft.AspNetCore.Components.Forms;

namespace BackOffice.Client.Pages.ProductItemPage;

public partial class TypeDetails
{
    [Inject] private TabsService _tabsService { get; set; } = null!;

    [Inject] public IProductCatalogService CatalogService { get; set; } = null!;

    [Parameter]
    public EventCallback<string> OnSaveClick { get; set; }

    [Parameter]
    public ItemType Type { get; set; } = null!;

    private EditContext? _editContext;

    protected override void OnInitialized()
    {
        _editContext = new EditContext(Type);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task SaveClick()
    {
        if (!_editContext?.Validate() ?? false)
            return;

        var result = await CatalogItemService.UpdateTypeAsync(Type);
        if (result is not null)
        {
            await OnSaveClick.InvokeAsync(null);
            Close();
        }
    }

    private async Task DeleteClick()
    {
        await CatalogItemService.DeleteTypeAsync(Type.Id, true);
        
        Close();
    }

    private void Close()
    {
        _tabsService.RemoveTab(_tabsService.Tabs?.ActivePanel);
    }
}
