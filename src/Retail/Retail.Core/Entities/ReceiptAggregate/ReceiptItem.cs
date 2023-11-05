using System.ComponentModel.DataAnnotations;

namespace Retail.Core.Entities.ReceiptAggregate;

public class ReceiptItem: BaseEntity
{
    [Required]
    public int ReceiptId { get; set; }

    public Receipt Receipt { get; set; } = null!;

    [Required]
    public int Number { get; set; }

    [Required]
    public int CatalogItemId { get; set; }

    public CatalogItem CatalogItem { get; set; } = null!;

    [Required]
    public double Quantity { get; set; }

    [Required]
    public decimal UnitPrice { get; set; }

    [Required]
    public decimal TotalPrice { get; set; }
}
