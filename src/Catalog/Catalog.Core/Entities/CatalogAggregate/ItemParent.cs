namespace Catalog.Core.Entities.CatalogAggregate;

public class ItemParent : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public Guid? ParentId { get; set; }

    public ItemParent? Parent { get; set; }
}
