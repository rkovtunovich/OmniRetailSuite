namespace Contracts.Dtos.ProductCatalog;

public record ItemDto : EntityDtoBase
{
    public string Name { get; init; } = null!;

    public int CodeNumber { get; init; }

    public string? CodePrefix { get; init; }

    public string Description { get; init; } = string.Empty;

    public decimal Price { get; init; }

    public string PictureUri { get; init; } = string.Empty;

    public string PictureBase64 { get; init; } = string.Empty;

    public Guid? ParentId { get; init; }

    public ItemTypeDto? CatalogType { get; init; }

    public BrandDto? CatalogBrand { get; init; }

    public string? Barcode { get; init; }
}
