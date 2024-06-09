using AutoMapper;
using BackOffice.Core.Models.ProductCatalog;
using Microsoft.AspNetCore.Components.Web;

namespace BackOffice.Client.Components.Pages.ProductCatalog.Item;

public partial class ItemDetails: DetailsFormBase<ProductItem>
{
    #region Injects

    [Inject] public IProductCatalogService<ProductItem> ProductItemService { get; set; } = null!;

    [Inject] public IProductCatalogService<ProductType> ProductTypeService { get; set; } = null!;

    [Inject] public IProductCatalogService<ProductBrand> ProductBrandService { get; set; } = null!;

    [Inject] public IProductCatalogService<ProductParent> ProductParentService { get; set; } = null!;

    [Inject] public IMapper Mapper { get; set; } = null!;

    #endregion

    #region Parameters

    [Parameter]
    public EventCallback<string> OnSaveClick { get; set; }

    #endregion

    #region Fields

    private IList<ProductType> _itemTypes = [];

    private IList<ProductBrand> _itemBrands = [];

    private IList<ProductParent> _allProductParents = [];

    #endregion

    #region Overrides

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _allProductParents = await ProductParentService.GetAllAsync();
            _itemTypes = await ProductTypeService.GetAllAsync();
            _itemBrands = await ProductBrandService.GetAllAsync();

            CallRequestRefresh();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    protected override async Task<ProductItem> GetModel()
    {
        return await ProductItemService.GetByIdAsync(Id) ?? new();
    }

    #endregion

    #region Private Methods

    #region Commands

    protected override void DefineFormToolbarCommands()
    {
        ToolbarCommands =
        [
            new() {
                Name = "Save",
                Icon = Icons.Material.Outlined.Save,
                Callback = EventCallback.Factory.Create<MouseEventArgs>(this, SaveClick),
                Tooltip = "Save"
            },
            new() {
                Name = "Delete",
                Icon = Icons.Material.Outlined.Delete,
                Callback = EventCallback.Factory.Create<MouseEventArgs>(this, DeleteClick),
                Tooltip = "Delete"
            },
            new() {
                Name = "Close",
                Icon = Icons.Material.Outlined.Close,
                Callback = EventCallback.Factory.Create<MouseEventArgs>(this, CloseClick),
                Tooltip = "Close"
            }
        ];
    }

    #endregion

    #region Clicks

    private async Task SaveClick(MouseEventArgs mouseEventArgs)
    {
        if (!EditContext?.Validate() ?? false)
            return;
        
        var result = await ProductItemService.UpdateAsync(Model);
        if (result)
        {
            await OnSaveClick.InvokeAsync(null);
            CloseClick();
        }
    }

    private async Task DeleteClick()
    {
        await ProductItemService.DeleteAsync(Model.Id, true);

        CloseClick();
    }

    #endregion

    #region Product parent

    private void OnParentChanged(HierarchySelectModel selectModel)
    {
        Model.ParentId = selectModel?.Id;
    }

    #endregion

    #endregion

    #region Delegates

    private Func<ProductType, string> _converterItemType = p => p?.Name ?? "";

    private Func<ProductBrand, string> _converterItemBrand = p => p?.Name ?? "";

    #endregion
}
