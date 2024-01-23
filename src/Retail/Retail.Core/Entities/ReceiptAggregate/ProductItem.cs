namespace Retail.Core.Entities.ReceiptAggregate;

public class ProductItem: EntityBase, ICodedEntity
{
    public int CodeNumber { get; set; }

    public string? CodePrefix { get; set; }

    public string Name { get; set; } = null!;
}
