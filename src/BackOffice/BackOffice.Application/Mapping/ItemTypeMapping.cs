using BackOffice.Core.Models.Product;

namespace BackOffice.Application.Mapping;

public static class ItemTypeMapping
{
    public static ItemTypeDto? ToDto(this ItemType catalogType)
    {
        return new ItemTypeDto
        {
            Id = catalogType.Id,
            Name = catalogType.Name
        };
    }

    public static ItemType ToModel(this ItemTypeDto catalogType)
    {
        return new ItemType
        {
            Id = catalogType.Id,
            Name = catalogType.Name
        };
    }
}
