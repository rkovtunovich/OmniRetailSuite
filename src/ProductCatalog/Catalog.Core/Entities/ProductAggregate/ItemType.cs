namespace ProductCatalog.Core.Entities.ProductAggregate;

public class ItemType : BaseEntity, IAggregateRoot, ICodedEntity
{
    public string? CodePrefix { get; set; }

    public int CodeNumber { get; set; }

    public string Type { get; set; } = string.Empty;

    public ItemType(string type)
    {
        Type = type;
    }

    public ItemType()
    {
    }
}
