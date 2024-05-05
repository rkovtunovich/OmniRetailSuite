namespace Contracts.Dtos.ProductCatalog;

public record ProductItemDto : EntityDtoBase
{
    public string Name { get; init; } = null!;

    public int CodeNumber { get; init; }

    public string? CodePrefix { get; init; }

    public string Description { get; init; } = string.Empty;

    public decimal Price { get; init; }

    public string PictureUri { get; init; } = string.Empty;

    public string PictureBase64 { get; init; } = string.Empty;

    public string? Barcode { get; init; }

    public Guid? ParentId { get; init; }

    public ProductTypeDto? ProductType { get; init; }

    public ProductBrandDto? ProductBrand { get; init; }
}
