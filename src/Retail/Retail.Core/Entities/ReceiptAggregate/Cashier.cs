using System.ComponentModel.DataAnnotations;

namespace Retail.Core.Entities.ReceiptAggregate;

public class Cashier() : BaseEntity
{
    public string Name { get; set; } = null!;
}
