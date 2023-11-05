using Catalog.Application.DTOs.CatalogTDOs;
using Catalog.Core.Entities.CatalogAggregate;

namespace Catalog.Application.DTOs.Mapping;

public static class BrandMapping
{
    public static BrandDto? ToDto(this Brand catalogBrand)
    {
        return new BrandDto
        {
            Id = catalogBrand.Id,
            Name = catalogBrand.Name
        };
    }
}
