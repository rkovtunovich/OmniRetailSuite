using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.Core.Entities.ProductAggregate;

public class ProductItem : EntityBase, IAggregateRoot, ICodedEntity
{
    public string Name { get; set; } = null!;

    public string? CodePrefix { get; set; }

    public int CodeNumber { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string PictureUri { get; set; } = string.Empty;

    public ProductParent? Parent { get; set; }

    public Guid? ParentId { get; set; }

    [StringLength(13)]
    public string? Barcode { get; set; }

    public Guid? ProductTypeId { get; set; }

    public virtual ProductType? ProductType { get; set; }

    public Guid? ProductBrandId { get; set; }

    public virtual ProductBrand? ProductBrand { get; set; }
}
