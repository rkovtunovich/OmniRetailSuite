using Infrastructure.Common.Services;
using Microsoft.AspNetCore.Components.Web;
using RetailAssistant.Core.Models.ProductCatalog;
using RetailAssistant.Data;
using UI.Razor.Enums;
using UI.Razor.Helpers;

namespace RetailAssistant.Client.Components.Pages.CashiersWorkplace;

public partial class CashiersMain
{
    #region Injected services

    [Inject] public IApplicationRepository<ProductParent, ProductCatalogDbSchema> ProductParentRepository { get; set; } = null!;

    [Inject] public IApplicationRepository<CatalogProductItem, ProductCatalogDbSchema> ProductItemRepository { get; set; } = null!;

    [Inject] public IApplicationRepository<Receipt, RetailDbSchema> ReceiptRepository { get; set; } = null!;

    [Inject] private ILocalConfigService LocalConfigService { get; set; } = null!;

    [Inject] private IGuidGenerator GuidGenerator { get; set; } = null!;

    [Inject] private IDialogService DialogService { get; set; } = null!;

    [Inject] private IStringLocalizer<CashiersMain> _localizer { get; set; } = default!;

    #endregion

    [Parameter]
    public Cashier? Cashier { get; set; }

    public IDialogReference? PaymentDialog { get; private set; }

    private double _splitterPercentage = 75;

    #region Overrides

    protected override void OnInitialized()
    {
        _tableHeadings = SetTableHeadings();
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
    private string[] _tableHeadings = [];
    private int _selectedRowNumber = -1;
    private MudTable<CatalogProductItem> _productItemsTable = null!;
    private bool _isTableLoading = false;

    #endregion

    #region methods

    private async Task ReloadProductItems(ProductParent? productParent = null)
    {
        if (productParent is null)
        {
            var productItemsList = await ProductItemRepository.GetAllAsync();
            _productItems = [.. productItemsList];
        }
        else
        {
            var productItemsList = await ProductItemRepository.GetAllByPropertyAsync(nameof(CatalogProductItem.ParentId), productParent.Id);
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

    private string[] SetTableHeadings()
    {
        return [_localizer["Code"], _localizer["Name"], _localizer["Price"]];
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
        var itemParentsList = await ProductParentRepository.GetAllAsync();
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
                Tooltip = _localizer["Clear"]
            },
            new ToolbarCommand
            {
                Name = "Payment",
                Icon = IconHelper.ShoppingCartCheckout,
                Callback = EventCallback.Factory.Create<MouseEventArgs>(this, TakeReceiptPayment),
                Tooltip = _localizer["Payment"],
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

    private async Task TakeReceiptPayment()
    {
        if (_receipt.TotalPrice is 0)
            return;

        var fragmentParameters = new Dictionary<string, object>
        {
            { nameof(ReceiptPayment.TotalPrice), _receipt.TotalPrice },
            { nameof(ReceiptPayment.OnPaymentCompleted), EventCallback.Factory.Create<decimal>(this, ReceiptPaymentMade) },
            { nameof(ReceiptPayment.OnPaymentCancelled), EventCallback.Factory.Create(this, ReceiptPaymentCancelled) }
        };

        var content = RenderFragmentBuilder.Create<ReceiptPayment>(fragmentParameters);

        var options = new DialogOptions
        {
            CloseOnEscapeKey = false,
            MaxWidth = MaxWidth.Medium,
            DisableBackdropClick = true
        };

        var dialogParameters = new DialogParameters { { "ChildContent", content } };

        PaymentDialog = await DialogService.ShowAsync<ModalComponent>(_localizer["Payment"], dialogParameters, options);
    }

    private async Task ReceiptPaymentMade(decimal paymentAmount)
    {
        if (paymentAmount < _receipt.TotalPrice)
            return;

        PaymentDialog?.Close();

        _receipt.Date = DateTime.Now;
        await ReceiptRepository.CreateAsync(_receipt);

        await InitNewReceipt();

        CallRequestRefresh();

        PaymentDialog = null;
    }

    private void ReceiptPaymentCancelled()
    {
        PaymentDialog?.Close();
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

        var dialog = DialogService.Show<ModalComponent>(_localizer["CashierSelection"], dialogParameters, options);
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
