namespace ProductCatalog.Core.Entities.ProductAggregate;

public class Brand : BaseEntity, IAggregateRoot, ICodedEntity
{
    public string Name { get; set; } = string.Empty;

    public string? CodePrefix { get; set; }

    public int CodeNumber { get; set; }

    public Brand(string brand)
    {
        Name = brand;
    }

    public Brand()
    {
    }
}
