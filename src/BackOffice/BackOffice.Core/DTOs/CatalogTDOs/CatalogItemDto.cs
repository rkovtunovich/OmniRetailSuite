using BackOffice.Core.DTOs.Mapping;
using BackOffice.Core.Models.Catalog;
using System.Linq.Expressions;

namespace BackOffice.Core.DTOs.CatalogTDOs;

public record CatalogItemDto
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public decimal Price { get; init; }

    public string? Barcode { get; init; }

    public string PictureUri { get; init; } = string.Empty;

    public string PictureBase64 { get; init; } = string.Empty;

    public int CatalogTypeId { get; init; }

    public CatalogTypeDto? CatalogType { get; init; }

    public int CatalogBrandId { get; init; }

    public CatalogBrandDto CatalogBrand { get; init; }

    //public static Expression<Func<CatalogItem, CatalogItemDto>> Projection
    //{
    //    get
    //    {
    //        return item => new CatalogItemDto
    //        {
    //            Id = item.Id,
    //            Name = item.Name,
    //            Description = item.Description,
    //            Price = item.Price,
    //            Barcode = item.Barcode,
    //            PictureUri = item.PictureUri,
    //            CatalogTypeId = item.CatalogTypeId,
    //            CatalogType = item.CatalogType == null ? null : item.CatalogType.ToDto(),
    //            CatalogBrandId = item.CatalogBrandId,
    //            CatalogBrand = item.CatalogBrand == null ? null : item.CatalogBrand.ToDto()
    //        };
    //    }
    //}

    public CatalogItem ToModel()
    {
        return new CatalogItem()
        {
            Id = Id,
            Name = Name,
            Description = Description,
            Price = Price,
            Barcode = Barcode,
            PictureUri = PictureUri,
            CatalogTypeId = CatalogTypeId,
            CatalogBrandId = CatalogBrandId
        };
    }
}
