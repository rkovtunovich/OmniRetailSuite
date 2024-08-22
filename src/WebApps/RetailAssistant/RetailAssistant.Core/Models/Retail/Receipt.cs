namespace RetailAssistant.Core.Models.Retail;

public class Receipt : EntityModelBase
{
    private Cashier _cashier = null!;

    private Store _store = null!;

    public DateTimeOffset Date { get; set; }

    public Guid StoreId { get; set; }

    // when store is set, it should be set to the same value as StoreId
    public Store Store
    {
        get => _store;
        set
        {
            _store = value;

            if (value is not null)
                StoreId = value.Id;
            else
                StoreId = Guid.Empty;
        }
    }

    public Guid CashierId { get; set; }

    // when cashier is set, it should be set to the same value as CashierId
    public Cashier Cashier
    {
        get => _cashier;
        set
        {
            _cashier = value;

            if (value is not null)
                CashierId = value.Id;
            else
                CashierId = Guid.Empty;
        }
    }

    public decimal TotalPrice { get; set; }

    public List<ReceiptItem> ReceiptItems { get; set; } = [];

    public void AddReceiptItem(ReceiptItem receiptItem)
    {
        ReceiptItems.Add(receiptItem);

        TotalPrice += receiptItem.TotalPrice;
    }

    public void RemoveReceiptItem(ReceiptItem receiptItem)
    {
        ReceiptItems.Remove(receiptItem);

        TotalPrice -= receiptItem.TotalPrice;
    }

    public bool TryUpdateReceiptItemByCatalogItem(CatalogProductItem catalogProductItem, double quantity = 1)
    {
        var receiptItem = ReceiptItems.FirstOrDefault(ri => ri.ProductItemId == catalogProductItem.Id);

        if (receiptItem is null)
            return false;

        receiptItem.Quantity += quantity;
        receiptItem.TotalPrice = (decimal)receiptItem.Quantity * catalogProductItem.Price;

        TotalPrice = ReceiptItems.Sum(ri => ri.TotalPrice);

        return true;
    }

    public void ClearReceiptItems()
    {
        ReceiptItems.Clear();

        TotalPrice = 0;
    }

    public ReceiptItem CreateReceiptItemByCatalogProductItem(Guid id, CatalogProductItem productItem)
    {
        return new ReceiptItem
        {
            Id = id,
            ProductItem = new RetailProductItem { Id = productItem.Id, Name = productItem.Name },
            ReceiptId = Id,
            Quantity = 1,
            UnitPrice = productItem.Price,
            TotalPrice = productItem.Price,
            LineNumber = ReceiptItems.Count + 1
        };
    }
}
