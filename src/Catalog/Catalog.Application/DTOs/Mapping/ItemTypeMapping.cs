using Catalog.Application.DTOs.CatalogTDOs;

namespace Catalog.Application.DTOs.Mapping;

public static class ItemTypeMapping
{
    public static ItemTypeDto? ToDto(this ItemType catalogType)
    {
        return new ItemTypeDto
        {
            Id = catalogType.Id,
            Name = catalogType.Type
        };
    }
}
