namespace Catalog.Core.Entities.CatalogAggregate;

public class Brand : BaseEntity, IAggregateRoot
{
    public string Name { get; set; } = string.Empty;

    public Brand(string brand)
    {
        Name = brand;
    }

    public Brand()
    {
    }
}
