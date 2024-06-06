using BackOffice.Core.Models.ProductCatalog;

namespace BackOffice.Core.Models.Retail;

public class ReceiptItem
{
    public Guid Id { get; set; }

    public Guid ReceiptId { get; set; }

    public int LineNumber { get; set; }

    public RetailProductItem ProductItem { get; set; } = null!;

    public double Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }
}

