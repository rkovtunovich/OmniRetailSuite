using BackOffice.Application.Services.Abstraction.ProductCatalog;
using BackOffice.Client.Services;
using BackOffice.Core.Models.ProductCatalog;
using Microsoft.AspNetCore.Components.Forms;

namespace BackOffice.Client.Pages.ProductCatalog.Parent;

public partial class ItemParentDetails
{
    [Inject] private TabsService _tabsService { get; set; } = null!;

    [Inject] public IProductParentService ProductParentService { get; set; } = null!;

    [Parameter]
    public EventCallback<string> OnSaveClick { get; set; }

    [Parameter]
    public ProductParent ProductParent { get; set; } = null!;

    private EditContext? _editContext;

    protected override void OnInitialized()
    {
        _editContext = new EditContext(ProductParent);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task SaveClick()
    {
        if (!_editContext?.Validate() ?? false)
            return;

        var result = await ProductParentService.UpdateItemParentAsync(ProductParent);
        if (result is not null)
        {
            await OnSaveClick.InvokeAsync(null);
            Close();
        }
    }

    private async Task DeleteClick()
    {
        await ProductParentService.DeleteItemParentAsync(ProductParent.Id, true);

        Close();
    }

    private void Close()
    {
        _tabsService.RemoveTab(_tabsService.Tabs?.ActivePanel);
    }
}
