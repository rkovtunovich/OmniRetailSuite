using BackOffice.Core.DTOs.CatalogTDOs;
using BackOffice.Core.Models.Catalog;

namespace BackOffice.Core.DTOs.Mapping;

public static class ItemParentMapping
{
    public static ItemParentDto? ToDto(this ItemParent itemParent)
    {
        return new ItemParentDto
        {
            Id = itemParent.Id,
            Name = itemParent.Name
        };
    }
}
