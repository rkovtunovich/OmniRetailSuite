namespace RetailAssistant.Core.Models.Retail;

public class ReceiptItem
{
    private RetailProductItem _productItem = null!;

    public Guid Id { get; set; }

    public Guid ReceiptId { get; set; }

    public int LineNumber { get; set; }

    public Guid ProductItemId { get; set; }

    // when ProductItem is set, it should be set to the same value as ProductItemId
    public RetailProductItem ProductItem
    {
        get => _productItem;
        set
        {
            _productItem = value;

            if (value is not null)
                ProductItemId = value.Id;
            else
                ProductItemId = Guid.Empty;
        }
    }

    public double Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }
}
