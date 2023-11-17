using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.Core.Entities.ProductAggregate;

public class Item : BaseEntity, IAggregateRoot
{
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public string? PictureFileName { get; set; }

    public string PictureUri { get; set; } = string.Empty;

    public ItemParent? Parent { get; set; }

    public Guid? ParentId { get; set; }

    public Guid? CatalogTypeId { get; set; }

    [StringLength(13)]
    public string? Barcode { get; set; }

    public virtual ItemType? CatalogType { get; set; }

    public Guid? CatalogBrandId { get; set; }

    public virtual Brand? CatalogBrand { get; set; }

    public readonly record struct CatalogItemDetails
    {
        public string? Name { get; }

        public string? Description { get; }

        public decimal Price { get; }

        public CatalogItemDetails(string? name, string? description, decimal price)
        {
            Name = name;
            Description = description;
            Price = price;
        }
    }
}
