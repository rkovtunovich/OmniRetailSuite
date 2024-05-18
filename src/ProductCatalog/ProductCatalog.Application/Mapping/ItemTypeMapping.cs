namespace ProductCatalog.Application.Mapping;

public static class ItemTypeMapping
{
    public static ProductTypeDto ToDto(this ItemType catalogType)
    {
        return new ProductTypeDto
        {
            Id = catalogType.Id,
            Name = catalogType.Type,
            CodeNumber = catalogType.CodeNumber,
            CodePrefix = catalogType.CodePrefix
        };
    }

    public static ItemType ToEntity(this ProductTypeDto catalogTypeDto)
    {
        return new ItemType
        {
            Id = catalogTypeDto.Id,
            Type = catalogTypeDto.Name,
            CodeNumber = catalogTypeDto.CodeNumber,
            CodePrefix = catalogTypeDto.CodePrefix
        };
    }
}
