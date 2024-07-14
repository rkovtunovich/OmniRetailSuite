namespace ProductCatalog.Core.Entities.ProductAggregate;

public class ProductBrand : EntityBase, IAggregateRoot, ICodedEntity
{
    public string Name { get; set; } = string.Empty;

    public string? CodePrefix { get; set; }

    public int CodeNumber { get; set; }

    public ProductBrand(string brand)
    {
        Name = brand;
    }

    public ProductBrand()
    {
    }
}
