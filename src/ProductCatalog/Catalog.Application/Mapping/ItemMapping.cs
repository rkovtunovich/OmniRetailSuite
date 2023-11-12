namespace ProductCatalog.Application.Mapping;

public static class ItemMapping
{
    public static ItemDto ToDto(this Item item)
    {
        return new ItemDto
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Price = item.Price,
            PictureUri = item.PictureUri,
            CatalogBrand = item.CatalogBrand?.ToDto(),
            CatalogType = item.CatalogType?.ToDto()
        };
    }

    public static Item ToEntity(this ItemDto itemDto)
    {
        return new Item
        {
            Id = itemDto.Id,
            Name = itemDto.Name,
            Description = itemDto.Description,
            Price = itemDto.Price,
            PictureUri = itemDto.PictureUri,
            CatalogBrand = itemDto?.CatalogBrand?.ToEntity(),
            CatalogType = itemDto?.CatalogType?.ToEntity()
        };
    }
}
