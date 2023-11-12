namespace Contracts.Dtos.ProductCatalog;

public record ItemDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public decimal Price { get; init; }

    public string PictureUri { get; init; } = string.Empty;

    public string PictureBase64 { get; init; } = string.Empty;

    public Guid CatalogTypeId { get; init; }

    public ItemTypeDto? CatalogType { get; init; }

    public Guid CatalogBrandId { get; init; }

    public BrandDto? CatalogBrand { get; init; }

    public string? Barcode { get; init; }
}
