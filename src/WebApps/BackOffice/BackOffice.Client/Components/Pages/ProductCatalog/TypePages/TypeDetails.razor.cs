using BackOffice.Core.Models.ProductCatalog;

namespace BackOffice.Client.Components.Pages.ProductCatalog.TypePages;

public partial class TypeDetails: DetailsFormBase<ProductType>
{
    [Inject] public IProductCatalogService<ProductType> ProductTypeService { get; set; } = null!;

    [Inject] private IStringLocalizer<TypeDetails> _localizer { get; set; } = default!;

    [Parameter]
    public EventCallback<string> OnSaveClick { get; set; }

    protected override async Task<ProductType> GetModel()
    {
        return await ProductTypeService.GetByIdAsync(Id) ?? new();
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

    private async Task SaveClick()
    {
        if (!EditContext?.Validate() ?? false)
            return;

        var result = await ProductTypeService.UpdateAsync(Model);
        if (result)
        {
            await OnSaveClick.InvokeAsync(null);
            CloseClick();
        }
    }

    private async Task DeleteClick()
    {
        await ProductTypeService.DeleteAsync(Model.Id, true);

        CloseClick();
    }

    #endregion
}
