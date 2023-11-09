using Catalog.Application.DTOs.Mapping;

namespace Catalog.Application.DTOs.CatalogTDOs;

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

    public static Expression<Func<Item, ItemDto>> Projection
    {
        get
        {
            return item => new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                Barcode = item.Barcode,
                PictureUri = item.PictureUri,
                CatalogTypeId = item.CatalogTypeId,
                CatalogType = item.CatalogType == null ? null : item.CatalogType.ToDto(),
                CatalogBrandId = item.CatalogBrandId,
                CatalogBrand = item.CatalogBrand == null ? null : item.CatalogBrand.ToDto()
            };
        }
    }

    public Item ToEntity()
    {
        return new Item(CatalogTypeId, CatalogBrandId, Description, Name, Price, PictureUri)
        {
            Id = Id,
            Barcode = Barcode,
            CatalogBrand = CatalogBrand?.ToEntity(),
            CatalogType = CatalogType?.ToEntity(),
            CatalogTypeId = CatalogType?.Id ?? Guid.Empty,
            CatalogBrandId = CatalogBrand?.Id ?? Guid.Empty
        };
    }

    public static ItemDto? FromEntity(Item? entity)
    {
        if (entity is null)
            return null;

        return Projection.Compile().Invoke(entity);
    }


}
