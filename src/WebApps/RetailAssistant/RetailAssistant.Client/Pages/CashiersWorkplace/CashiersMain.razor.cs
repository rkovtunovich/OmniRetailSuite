using Infrastructure.Common.Services;
using Microsoft.AspNetCore.Components.Web;
using RetailAssistant.Core.Models.ProductCatalog;
using UI.Razor.Enums;
using UI.Razor.Helpers;

namespace RetailAssistant.Client.Pages.CashiersWorkplace;

public partial class CashiersMain
{
    #region Injected services

    [Inject] public IProductCatalogService<ProductParent> ProductParentService { get; set; } = null!;

    [Inject] public IProductCatalogService<CatalogProductItem> ProductItemService { get; set; } = null!;

    [Inject] public IRetailService<Receipt> ReceiptService { get; set; } = null!;

    [Inject] private ILocalConfigService LocalConfigService { get; set; } = null!;

    [Inject] private IGuidGenerator GuidGenerator { get; set; } = null!;

    [Inject] private IDialogService DialogService { get; set; } = null!;

    #endregion

    [Parameter]
    public Cashier? Cashier { get; set; }

    private double _splitterPercentage = 75;

    #region Overrides

    protected override void OnInitialized()
    {
        InitializeReceiptCommands();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            await ReloadProductItems();
            await ReloadItemParents();

            if (Cashier is null)
                await ShowCashierChangeDialog();

            await InitNewReceipt();
        }
    }

    #endregion

    #region Product Items

    #region fields

    private List<CatalogProductItem> _productItems = [];
    private string[] _tableHeadings = ["Code", "Name", "Price"];
    private int _selectedRowNumber = -1;
    private MudTable<CatalogProductItem> _productItemsTable = null!;
    private bool _isTableLoading = false;

    #endregion

    #region methods

    private async Task ReloadProductItems(ProductParent? productParent = null)
    {
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

    private async void OnParentItemClick(ProductParent productParent, MouseEventArgs mouseEventArgs)
    {
        _selectedProductParent = productParent;
        _isTableLoading = true;

        if (productParent.Name == FilterSpecialCase.All.ToString())
        {
            await ReloadProductItems();
            _selectedProductParent = null;
        }
        else
        {
            await ReloadProductItems(productParent);
        }

        _isTableLoading = false;

        StateHasChanged();
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

    private void InitializeReceiptCommands()
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
                Tooltip = "Save",
                CssClass = "cashiers-receipt-save-button"
            }
        ];
    }

    private void ClearReceipt()
    {
        _receipt.ClearReceiptItems();
    }

    public void AddProductItemToReceipt(CatalogProductItem productItem)
    {
        if (!_receipt.TryUpdateReceiptItemByCatalogItem(productItem))
        {
            var receiptItem = _receipt.CreateReceiptItemByCatalogProductItem(GuidGenerator.Create(), productItem);
            _receipt.AddReceiptItem(receiptItem);
        }
    }

    private async Task SaveReceipt()
    {
        _receipt.Date = DateTime.Now;
        await ReceiptService.CreateAsync(_receipt);

        await InitNewReceipt();

        CallRequestRefresh();
    }

    private async Task InitNewReceipt()
    {
        ClearReceipt();
        var localSettings = await LocalConfigService.GetConfigAsync();

        _receipt = new()
        {
            Id = GuidGenerator.Create(),
            Store = localSettings?.Store ?? throw new ArgumentNullException(nameof(localSettings.Store)),
        };

        if (Cashier is not null)
            _receipt.Cashier = Cashier;
    }

    #endregion

    #endregion

    #region Cashier


    private async Task ShowCashierChangeDialog()
    {
        var localSettings = await LocalConfigService.GetConfigAsync();
        var store = localSettings?.Store ?? throw new ArgumentNullException(nameof(localSettings.Store));

        var fragmentParameters = new Dictionary<string, object>
        {
            { nameof(CashierSelectionList.Items), store.Cashiers },
            { nameof(CashierSelectionList.OnSingleSelectionMade), EventCallback.Factory.Create<Cashier>(this, CashierOnSelectionChanged) }
        };

        var content = RenderFragmentBuilder.Create<CashierSelectionList>(fragmentParameters);

        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Medium,
            CloseButton = true,

        };
        var dialogParameters = new DialogParameters { { "ChildContent", content } };

        var dialog = DialogService.Show<ModalComponent>("Select cashier", dialogParameters, options);
    }

    private void CashierOnSelectionChanged(Cashier selectedCashier)
    {
        if (selectedCashier is null)
            return;

        Cashier = selectedCashier;
        _receipt.Cashier = selectedCashier;
    }

    #endregion
}
