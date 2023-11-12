using BackOffice.Core.Models.Product;

namespace BackOffice.Application.Mapping;

public static class ItemMapping
{
    public static Item ToModel(this ItemDto dto)
    {
        return new Item()
        {
            Id = dto.Id,
            Brand = dto.CatalogBrand?.ToModel(),
            CatalogBrandId = dto.CatalogBrandId,
            ItemType = dto.CatalogType?.ToModel(),
            ItemTypeId = dto.CatalogTypeId,
            Description = dto.Description,
            Name = dto.Name,
            PictureUri = dto.PictureUri,
            Price = dto.Price,
            Barcode = dto.Barcode,
        };
    }

    public static ItemDto ToDto(this Item model)
    {
        return new ItemDto()
        {
            Id = model.Id,
            CatalogBrand = model.Brand?.ToDto(),
            CatalogBrandId = model.CatalogBrandId,
            CatalogType = model.ItemType?.ToDto(),
            CatalogTypeId = model.ItemTypeId,
            Description = model.Description,
            Name = model.Name,
            PictureUri = model.PictureUri,
            Price = model.Price,
            Barcode = model.Barcode,
        };
    }
}
