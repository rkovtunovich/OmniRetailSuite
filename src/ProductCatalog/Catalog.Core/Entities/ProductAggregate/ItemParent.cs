namespace ProductCatalog.Core.Entities.ProductAggregate;

public class ItemParent : BaseEntity, ICodedEntity
{
    public string? CodePrefix { get; set; }

    public int CodeNumber { get; set; }

    public string Name { get; set; } = string.Empty;

    public Guid? ParentId { get; set; }

    public ItemParent? Parent { get; set; }

    public List<ItemParent>? Children { get; set; }
}
