using BackOffice.Core.Models.ProductCatalog;

namespace BackOffice.Application.Mapping;

public static class ItemMapping
{
    public static ProductItem ToModel(this ItemDto dto)
    {
        return new ProductItem()
        {
            Id = dto.Id,
            Brand = dto.CatalogBrand?.ToModel(),
            CatalogBrandId = dto.CatalogBrand?.Id,
            ItemType = dto.CatalogType?.ToModel(),
            ItemTypeId = dto.CatalogType?.Id,
            Description = dto.Description,
            Name = dto.Name,
            PictureUri = dto.PictureUri,
            Price = dto.Price,
            Barcode = dto.Barcode,
        };
    }

    public static ItemDto ToDto(this ProductItem model)
    {
        return new ItemDto()
        {
            Id = model.Id,
            CatalogBrand = model.Brand?.ToDto(),   
            CatalogType = model.ItemType?.ToDto(),
            Description = model.Description,
            Name = model.Name,
            PictureUri = model.PictureUri,
            Price = model.Price,
            Barcode = model.Barcode,
        };
    }
}
