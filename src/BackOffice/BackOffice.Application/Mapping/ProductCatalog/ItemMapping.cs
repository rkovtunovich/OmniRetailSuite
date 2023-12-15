using BackOffice.Core.Models.ProductCatalog;

namespace BackOffice.Application.Mapping.ProductCatalog;

public static class ItemMapping
{
    public static ProductItem ToModel(this ItemDto dto)
    {
        return new ProductItem()
        {
            Id = dto.Id,
            Name = dto.Name,
            CodeNumber = dto.CodeNumber,
            CodePrefix = dto.CodePrefix,
            ParentId = dto.ParentId,
            Brand = dto.CatalogBrand?.ToModel(),
            CatalogBrandId = dto.CatalogBrand?.Id,
            ItemType = dto.CatalogType?.ToModel(),
            ItemTypeId = dto.CatalogType?.Id,
            Description = dto.Description,
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
            Name = model.Name,
            CodeNumber = model.CodeNumber,
            CodePrefix = model.CodePrefix,
            ParentId = model.ParentId,
            CatalogBrand = model.Brand?.ToDto(),
            CatalogType = model.ItemType?.ToDto(),
            Description = model.Description,
            PictureUri = model.PictureUri,
            Price = model.Price,
            Barcode = model.Barcode,
        };
    }
}
