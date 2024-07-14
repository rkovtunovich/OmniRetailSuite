using BackOffice.Core.Models.ProductCatalog;

namespace BackOffice.Client.Components.Pages.ProductCatalog.TypePages;

public partial class TypeCreate: CreationFormBase<ProductType>
{
    [Inject] public IProductCatalogService<ProductType> ProductTypeService { get; set; } = null!;

    [Inject] private IStringLocalizer<TypeCreate> _localizer { get; set; } = default!;

    [Parameter]
    public EventCallback<string> OnSaveClick { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task CreateClick()
    {
        if (!EditContext?.Validate() ?? false)
            return;

        var result = await ProductTypeService.CreateAsync(Model);
        if (result is not null)
        {
            await OnSaveClick.InvokeAsync(null);
            CloseClick();
        }
    }

    #region Commands

    protected override void DefineFormToolbarCommands()
    {
        ToolbarCommands =
        [
            new()
            {
                Name = "Save",
                Icon = Icons.Material.Outlined.Save,
                Callback = EventCallback.Factory.Create<MouseEventArgs>(this, CreateClick),
                Tooltip = _localizer["Save"]
            },
            new()
            {
                Name = "Close",
                Icon = Icons.Material.Outlined.Close,
                Callback = EventCallback.Factory.Create<MouseEventArgs>(this, CloseClick),
                Tooltip = _localizer["Close"]
            }
        ];
    }

    #endregion
}
