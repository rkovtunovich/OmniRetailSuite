using BackOffice.Core.Models.ProductCatalog;

namespace BackOffice.Client.Components.Pages.ProductCatalog.BrandPages;

public partial class BrandDetails: DetailsFormBase<ProductBrand>
{
    [Inject] public IProductCatalogService<ProductBrand> ProductBrandService { get; set; } = null!;

    [Inject] private IStringLocalizer<BrandDetails> _localizer { get; set; } = default!;

    [Parameter]
    public EventCallback<string> OnSaveClick { get; set; }

    private async Task SaveClick()
    {
        if (!EditContext?.Validate() ?? false)
            return;

        var result = await ProductBrandService.UpdateAsync(Model);
        if (result)
        {
            await OnSaveClick.InvokeAsync(null);
            CloseClick();
        }
    }

    protected override async Task<ProductBrand> GetModel()
    {
        return await ProductBrandService.GetByIdAsync(Id) ?? new();
    }

    private async Task DeleteClick()
    {
        await ProductBrandService.DeleteAsync(Model.Id, true);

        CloseClick();
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
                Callback = EventCallback.Factory.Create<MouseEventArgs>(this, SaveClick),
                Tooltip = _localizer["Save"]
            },
            new()
            {
                Name = "Delete",
                Icon = Icons.Material.Outlined.Delete,
                Callback = EventCallback.Factory.Create<MouseEventArgs>(this, DeleteClick),
                Tooltip = _localizer["Delete"]
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
