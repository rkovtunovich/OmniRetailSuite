﻿using ProductCatalog.Core.Entities.ProductAggregate;

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
            CatalogBrandId = itemDto?.CatalogBrand?.Id,
            CatalogBrand = itemDto?.CatalogBrand?.ToEntity(),
            CatalogTypeId = itemDto?.CatalogType?.Id,
            CatalogType = itemDto?.CatalogType?.ToEntity()
        };
    }
}
