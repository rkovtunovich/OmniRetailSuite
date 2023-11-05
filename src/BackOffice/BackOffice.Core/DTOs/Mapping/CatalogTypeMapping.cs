using BackOffice.Core.DTOs.CatalogTDOs;
using BackOffice.Core.Models.Catalog;

namespace BackOffice.Core.DTOs.Mapping;

public static class CatalogTypeMapping
{
    public static CatalogTypeDto? ToDto(this CatalogType catalogType)
    {
        return new CatalogTypeDto
        {
            Id = catalogType.Id,
            Name = catalogType.Name
        };
    }
}
