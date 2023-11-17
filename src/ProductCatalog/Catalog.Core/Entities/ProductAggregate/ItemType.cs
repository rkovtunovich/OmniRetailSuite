namespace ProductCatalog.Core.Entities.ProductAggregate;

public class ItemType : BaseEntity, IAggregateRoot
{
    public string Type { get; set; } = string.Empty;

    public ItemType(string type)
    {
        Type = type;
    }

    public ItemType()
    {
    }
}
