namespace ProductCatalog.Application.Mapping;

public static class ItemMapping
{
    public static ProductItemDto ToDto(this Item item)
    {
        return new ProductItemDto
        {
            Id = item.Id,
            Name = item.Name,
            CodeNumber = item.CodeNumber,
            CodePrefix = item.CodePrefix,
            Description = item.Description,
            Price = item.Price,
            PictureUri = item.PictureUri,
            ParentId = item.ParentId,
            CatalogBrand = item.CatalogBrand?.ToDto(),
            CatalogType = item.CatalogType?.ToDto(),
            Barcode = item.Barcode
        };
    }

    public static Item ToEntity(this ProductItemDto itemDto)
    {
        return new Item
        {
            Id = itemDto.Id,
            Name = itemDto.Name,
            CodeNumber = itemDto.CodeNumber,
            CodePrefix = itemDto.CodePrefix,
            Description = itemDto.Description,
            Price = itemDto.Price,
            PictureUri = itemDto.PictureUri,
            ParentId = itemDto.ParentId,
            CatalogBrandId = itemDto?.CatalogBrand?.Id,
            CatalogBrand = itemDto?.CatalogBrand?.ToEntity(),
            CatalogTypeId = itemDto?.CatalogType?.Id,
            CatalogType = itemDto?.CatalogType?.ToEntity(),
            Barcode = itemDto?.Barcode
        };
    }
}
