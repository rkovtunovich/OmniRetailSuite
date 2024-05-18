namespace ProductCatalog.Application.Mapping;

public static class ItemParentMapping
{
    public static ProductParentDto ToDto(this ItemParent itemParent)
    {
        return new ProductParentDto
        {
            Id = itemParent.Id,
            Name = itemParent.Name,
            CodeNumber = itemParent.CodeNumber,
            CodePrefix = itemParent.CodePrefix,
            ParentId = itemParent.ParentId,
            Children = itemParent.Children?.Select(x => ToDto(x))
        };
    }

    public static ItemParent ToEntity(this ProductParentDto itemParentDto)
    {
        return new ItemParent
        {
            Id = itemParentDto.Id,
            Name = itemParentDto.Name,
            CodeNumber = itemParentDto.CodeNumber,
            CodePrefix = itemParentDto.CodePrefix,
            ParentId = itemParentDto.ParentId
        };
    }
}
