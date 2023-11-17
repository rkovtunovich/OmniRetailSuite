namespace ProductCatalog.Core.Entities.ProductAggregate;

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
