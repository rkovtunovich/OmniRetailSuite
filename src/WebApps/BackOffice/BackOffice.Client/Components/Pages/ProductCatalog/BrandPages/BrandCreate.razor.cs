using BackOffice.Core.Models.ProductCatalog;

namespace BackOffice.Client.Components.Pages.ProductCatalog.BrandPages;

public partial class BrandCreate: CreationFormBase<ProductBrand>
{
    [Inject] public IProductCatalogService<ProductBrand> ProductBrandService { get; set; } = null!;

    [Inject] private IStringLocalizer<BrandCreate> _localizer { get; set; } = default!;

    [Parameter]
    public EventCallback<string> OnSaveClick { get; set; }

    private async Task CreateClick()
    {
        if (!EditContext?.Validate() ?? false)
            return;

        var result = await ProductBrandService.CreateAsync(Model);
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
