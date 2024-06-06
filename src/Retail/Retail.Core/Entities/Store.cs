namespace Retail.Core.Entities;

public class Store: EntityBase, ICodedEntity
{
    public int CodeNumber { get; set; }

    public string? CodePrefix { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public List<Cashier>? Cashiers { get; set; }
}
