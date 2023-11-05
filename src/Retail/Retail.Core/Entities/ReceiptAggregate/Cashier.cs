using System.ComponentModel.DataAnnotations;

namespace Retail.Core.Entities.ReceiptAggregate;

public class Cashier() : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
}
