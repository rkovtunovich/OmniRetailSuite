using BackOffice.Core.DTOs.CatalogTDOs;
using BackOffice.Core.Models.Catalog;

namespace BackOffice.Core.DTOs.Mapping;

public static class CatalogBrandMapping
{
    public static CatalogBrandDto? ToDto(this CatalogBrand catalogBrand)
    {
        return new CatalogBrandDto
        {
            Id = catalogBrand.Id,
            Name = catalogBrand.Name
        };
    }
}
