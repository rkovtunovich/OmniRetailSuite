namespace ProductCatalog.Core.Entities.ProductAggregate;

public class ProductParent : EntityBase, ICodedEntity
{
    public string? CodePrefix { get; set; }

    public int CodeNumber { get; set; }

    public string Name { get; set; } = string.Empty;

    public Guid? ParentId { get; set; }

    public ProductParent? Parent { get; set; }

    public List<ProductParent>? Children { get; set; }
}
