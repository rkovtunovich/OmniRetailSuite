namespace ProductCatalog.Core.Entities.ProductAggregate;

public class ProductType : EntityBase, IAggregateRoot, ICodedEntity
{
    public string? CodePrefix { get; set; }

    public int CodeNumber { get; set; }

    public string Name { get; set; } = string.Empty;

    public ProductType(string type)
    {
        Name = type;
    }

    public ProductType()
    {
    }
}
