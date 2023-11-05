using System.ComponentModel.DataAnnotations;

namespace Retail.Core.Entities.ReceiptAggregate;

public class Receipt: BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string Number { get; set; } = null!;

    public DateTimeOffset Date { get; set; }

    [Required]
    public int CashierId { get; set; }
    
    public Cashier Cashier { get; set; } = null!;

    [Required]
    public decimal TotalPrice { get; set; }

    public List<ReceiptItem> ReceiptItems { get; set; } = [];
}
