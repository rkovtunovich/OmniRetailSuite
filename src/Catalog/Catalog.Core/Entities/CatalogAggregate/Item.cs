using System.ComponentModel.DataAnnotations;
using Ardalis.GuardClauses;

namespace Catalog.Core.Entities.CatalogAggregate;

public class Item : BaseEntity, IAggregateRoot
{
    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public string? PictureFileName { get; set; }

    public string PictureUri { get; set; } = string.Empty;

    public ItemParent? Parent { get; set; }

    public int? ParentId { get; set; }

    public int CatalogTypeId { get; set; }

    [StringLength(13)]
    public string? Barcode { get; set; }

    public virtual ItemType? CatalogType { get; set; }

    public int CatalogBrandId { get; set; }

    public virtual Brand? CatalogBrand { get; set; }

    public Item(int catalogTypeId,
        int catalogBrandId,
        string description,
        string name,
        decimal price,
        string pictureUri)
    {
        CatalogTypeId = catalogTypeId;
        CatalogBrandId = catalogBrandId;
        Description = description;
        Name = name;
        Price = price;
        PictureUri = pictureUri;
    }

    public void UpdateName(string name)
    {
        Guard.Against.NullOrEmpty(name, nameof(name));
        Name = name;
    }

    public void UpdateDetails(CatalogItemDetails details)
    {
        Guard.Against.NullOrEmpty(details.Name, nameof(details.Name));
        Guard.Against.NullOrEmpty(details.Description, nameof(details.Description));
        Guard.Against.NegativeOrZero(details.Price, nameof(details.Price));

        Name = details.Name;
        Description = details.Description;
        Price = details.Price;
    }

    public void UpdateBrand(int catalogBrandId)
    {
        Guard.Against.Zero(catalogBrandId, nameof(catalogBrandId));
        CatalogBrandId = catalogBrandId;
    }

    public void UpdateType(int catalogTypeId)
    {
        Guard.Against.Zero(catalogTypeId, nameof(catalogTypeId));
        CatalogTypeId = catalogTypeId;
    }

    public void UpdatePictureUri(string pictureName)
    {
        if (string.IsNullOrEmpty(pictureName))
        {
            PictureUri = string.Empty;
            return;
        }
        PictureUri = $"images\\products\\{pictureName}?{new DateTime().Ticks}";
    }

    public string GetTypeName()
    {
        return CatalogType?.Type ?? string.Empty;
    }

    public string GetBrandName()
    {
        return CatalogBrand?.Name ?? string.Empty;
    }

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
