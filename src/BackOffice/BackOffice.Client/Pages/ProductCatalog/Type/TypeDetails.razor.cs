using BackOffice.Application.Services.Abstraction.ProductCatalog;
using BackOffice.Client.Services;
using BackOffice.Core.Models.ProductCatalog;
using Microsoft.AspNetCore.Components.Forms;

namespace BackOffice.Client.Pages.ProductCatalog.Type;

public partial class TypeDetails
{
    [Inject] private TabsService _tabsService { get; set; } = null!;

    [Inject] public IProductTypeService ProductTypeService { get; set; } = null!;

    [Parameter]
    public EventCallback<string> OnSaveClick { get; set; }

    [Parameter]
    public ProductType Type { get; set; } = null!;

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

        var result = await ProductTypeService.UpdateTypeAsync(Type);
        if (result is not null)
        {
            await OnSaveClick.InvokeAsync(null);
            Close();
        }
    }

    private async Task DeleteClick()
    {
        await ProductTypeService.DeleteTypeAsync(Type.Id, true);

        Close();
    }

    private void Close()
    {
        _tabsService.RemoveTab(_tabsService.Tabs?.ActivePanel);
    }
}
