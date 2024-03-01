using Infrastructure.Common.Services;
using Microsoft.AspNetCore.Components.Web;
using RetailAssistant.Core.Models.ProductCatalog;
using UI.Razor.Enums;
using UI.Razor.Models;

namespace RetailAssistant.Client.Pages.CashiersWorkplace;

public partial class CashiersWorkplace
{
    [Inject] public IProductCatalogService<ProductParent> ProductParentService { get; set; } = null!;

    [Inject] public IProductCatalogService<CatalogProductItem> ProductItemService { get; set; } = null!;

    [Inject] public IRetailService<Receipt> ReceiptService { get; set; } = null!;

    [Inject] private ILocalConfigService LocalConfigService { get; set; } = null!;

    [Inject] private IGuidGenerator _guidGenerator { get; set; } = null!; 

    private double _splitterPercentage = 75;

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        await OnInitializedReceipt();
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

    #region fields

    private List<CatalogProductItem> _productItems = [];
    private string[] _tableHeadings = ["Code", "Name", "Price"];
    private int _selectedRowNumber = -1;
    private MudTable<CatalogProductItem> _productItemsTable = null!;
    private bool _showLoading = false;

    #endregion

    #region methods

    private async Task ReloadProductItems(ProductParent? productParent = null)
    {
        _showLoading = true;
        if (productParent is null)
        {
            var productItemsList = await ProductItemService.GetAllAsync();
            _productItems = [.. productItemsList];
        }
        else
        {
            var productItemsList = await ProductItemService.GetByParentAsync(productParent.Id);
            _productItems = [.. productItemsList];
        }

        _showLoading = false;
        CallRequestRefresh();
    }

    private void RowClickEvent(TableRowClickEventArgs<CatalogProductItem> tableRowClickEventArgs)
    {
        if (tableRowClickEventArgs.Item is not null)
        {
            AddProductItemToReceipt(tableRowClickEventArgs.Item);
        }
    }

    private string SelectedRowClassFunc(CatalogProductItem productItem, int rowNumber)
    {
        if (_selectedRowNumber == rowNumber)
        {
            _selectedRowNumber = -1;
            return string.Empty;
        }
        else if (_productItemsTable.SelectedItem is not null && _productItemsTable.SelectedItem.Equals(productItem))
        {
            _selectedRowNumber = rowNumber;
            return "selected";
        }
        else
        {
            return string.Empty;
        }
    }

    #endregion

    #endregion

    #region Product parents

    #region fields

    private ProductParent? _selectedProductParent;
    private HashSet<ProductParent> _itemParents = [];
    private static readonly string selectedParentClassName = "ors-selected-parent-item";

    #endregion

    #region methods

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

    #endregion

    #region Receipt

    #region fields

    private Receipt _receipt = new();

    private List<ToolbarCommand> _receiptCommands = null!;

    #endregion

    #region methods

    private async Task OnInitializedReceipt()
    {
        _receiptCommands =
        [
            new ToolbarCommand
            {
                Name = "Clear",
                Icon = Icons.Material.Outlined.Clear,
                Callback = EventCallback.Factory.Create<MouseEventArgs>(this, ClearReceipt),
                Tooltip = "Clear"
            },
            new ToolbarCommand
            {
                Name = "Save",
                Icon = Icons.Material.Outlined.Save,
                Callback = EventCallback.Factory.Create<MouseEventArgs>(this, SaveReceipt),
                Tooltip = "Save"
            }
        ];

        await InitNewReceipt();     
    }

    private void ClearReceipt()
    {
        throw new NotImplementedException();
    }

    private void AddProductItemToReceipt(CatalogProductItem productItem)
    {
        if (!_receipt.TryUpdateReceiptItemByCatalogItem(productItem))
        {
            var receiptItem = _receipt.CreateReceiptItemByCatalogProductItem(_guidGenerator.Create(), productItem);
            _receipt.AddReceiptItem(receiptItem);
        }
    }

    private async void SaveReceipt()
    {
        _receipt.Date = DateTime.Now;

        await ReceiptService.CreateAsync(_receipt);

        await InitNewReceipt();
    }

    private async Task InitNewReceipt()
    {
        var localSettings = await LocalConfigService.GetConfigAsync();

        _receipt = new()
        {
            Id = _guidGenerator.Create(),
            Store = localSettings?.Store ?? throw new ArgumentNullException(nameof(localSettings.Store)),
            StoreId = localSettings.Store.Id,
        };
    }

    #endregion

    #endregion
}
