using BackOffice.Core.Models.ProductCatalog;

namespace BackOffice.Application.Mapping.ProductCatalog;

public static class ItemTypeMapping
{
    public static ItemTypeDto? ToDto(this ProductType catalogType)
    {
        return new ItemTypeDto
        {
            Id = catalogType.Id,
            Name = catalogType.Name,
            CodeNumber = catalogType.CodeNumber,
            CodePrefix = catalogType.CodePrefix
        };
    }

    public static ProductType ToModel(this ItemTypeDto catalogType)
    {
        return new ProductType
        {
            Id = catalogType.Id,
            Name = catalogType.Name,
            CodeNumber = catalogType.CodeNumber,
            CodePrefix = catalogType.CodePrefix
        };
    }
}
