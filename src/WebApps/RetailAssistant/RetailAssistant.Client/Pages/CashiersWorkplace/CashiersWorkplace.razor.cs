using Microsoft.AspNetCore.Components.Web;
using RetailAssistant.Core.Models.ProductCatalog;
using UI.Razor.Enums;

namespace RetailAssistant.Client.Pages.CashiersWorkplace;

public partial class CashiersWorkplace
{
    [Inject] public IProductCatalogService<ProductParent> ProductParentService { get; set; } = null!;

    [Inject] public IProductCatalogService<CatalogProductItem> ProductItemService { get; set; } = null!;

    private Receipt _receipt = null!;

    private double _splitterPercentage = 75;

    #region Product Items fields

    private List<CatalogProductItem> _productItems = [];

    #endregion

    #region Product Parents fields

    private ProductParent? _selectedProductParent;

    private HashSet<ProductParent> _itemParents = [];

    private static readonly string selectedParentClassName = "ors-selected-parent-item";

    #endregion

    #region Overrides

    protected override void OnInitialized()
    {
        _receipt = new Receipt();

        base.OnInitialized();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await ReloadProductItems();
            await ReloadItemParents(); ;
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    #endregion

    #region Product Items

    private async Task ReloadProductItems(ProductParent? productParent = null)
    {
        if (productParent == null)
        {
            var productItemsList = await ProductItemService.GetAllAsync();
            _productItems = [.. productItemsList];
        }
        else
        {
            var productItemsList = await ProductItemService.GetByParentAsync(productParent.Id);
            _productItems = [.. productItemsList];
        }

        CallRequestRefresh();
    }

    #endregion

    #region Product parents

    private async void OnParentItemDoubleClick(ProductParent productParent, MouseEventArgs mouseEventArgs)
    {
        _selectedProductParent = productParent;

        if (productParent.Name == FilterSpecialCase.All.ToString())
        {
            await ReloadProductItems();
            _selectedProductParent = null;
        }
        else
        {
            await ReloadProductItems(productParent);
        }
    }

    private async Task ReloadItemParents()
    {
        var itemParentsList = await ProductParentService.GetAllAsync();
        _itemParents.Clear();
        _itemParents.Add(new ProductParent { Id = Guid.NewGuid(), Name = FilterSpecialCase.All.ToString() });
        _itemParents.Add(new ProductParent { Id = Guid.NewGuid(), Name = FilterSpecialCase.Empty.ToString() });

        foreach (var itemParent in itemParentsList)
        {
            _itemParents.Add(itemParent);
        }

        CallRequestRefresh();
    }

    private string GetItemClass(ProductParent productParent)
    {
        if (_selectedProductParent is null)
            return "";

        if (_selectedProductParent.Name == FilterSpecialCase.All.ToString() && productParent.Name == _selectedProductParent.Name)
            return selectedParentClassName;

        if (_selectedProductParent.Name == FilterSpecialCase.Empty.ToString() && productParent.Name == _selectedProductParent.Name)
            return selectedParentClassName;

        return _selectedProductParent.Id == productParent.Id ? selectedParentClassName : "";
    }

    #endregion
}
