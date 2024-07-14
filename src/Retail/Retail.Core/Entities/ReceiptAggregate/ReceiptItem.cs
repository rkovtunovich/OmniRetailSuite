namespace Retail.Core.Entities.ReceiptAggregate;

public class ReceiptItem: EntityBase
{
    public Guid ReceiptId { get; set; }

    public Receipt Receipt { get; set; } = null!;

    public int LineNumber { get; set; }

    public Guid ProductItemId { get; set; }

    public ProductItem ProductItem { get; set; } = null!;

    public double Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }
}
