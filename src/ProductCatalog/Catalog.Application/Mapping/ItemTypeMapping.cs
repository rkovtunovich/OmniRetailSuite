namespace ProductCatalog.Application.Mapping;

public static class ItemTypeMapping
{
    public static ItemTypeDto ToDto(this ItemType catalogType)
    {
        return new ItemTypeDto
        {
            Id = catalogType.Id,
            Name = catalogType.Type,
            CodeNumber = catalogType.CodeNumber,
            CodePrefix = catalogType.CodePrefix
        };
    }

    public static ItemType ToEntity(this ItemTypeDto catalogTypeDto)
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
