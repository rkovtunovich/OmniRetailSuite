namespace RetailAssistant.Core.Models.Retail;

public class Receipt : EntityModelBase
{
    public DateTimeOffset Date { get; set; }

    public Guid StoreId { get; set; }

    public Store Store { get; set; } = null!;

    public Guid CashierId { get; set; }

    public Cashier Cashier { get; set; } = null!;

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

    public ReceiptItem CreateReceiptItemByCatalogProductItem(CatalogProductItem productItem)
    {
        return new ReceiptItem
        {
            ProductItemId = productItem.Id,
            ProductItem = new RetailProductItem { Id = productItem.Id, Name = productItem.Name },
            Receipt = this,
            ReceiptId = Id,
            Quantity = 1,
            UnitPrice = productItem.Price,
            TotalPrice = productItem.Price,
            LineNumber = ReceiptItems.Count + 1
        };
    }
}
