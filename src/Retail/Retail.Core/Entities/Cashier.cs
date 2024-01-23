namespace Retail.Core.Entities;

public class Cashier : EntityBase, ICodedEntity
{
    public string Name { get; set; } = null!;

    public int CodeNumber { get; set; }

    public string? CodePrefix { get; set; }
}
