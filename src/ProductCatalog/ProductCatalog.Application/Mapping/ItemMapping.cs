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
            ProductBrand = item.CatalogBrand?.ToDto(),
            ProductType = item.CatalogType?.ToDto(),
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
            CatalogBrandId = itemDto?.ProductBrand?.Id,
            CatalogBrand = itemDto?.ProductBrand?.ToEntity(),
            CatalogTypeId = itemDto?.ProductType?.Id,
            CatalogType = itemDto?.ProductType?.ToEntity(),
            Barcode = itemDto?.Barcode
        };
    }
}
