using BackOffice.Core.Models.ProductCatalog;

namespace BackOffice.Application.Mapping;

public static class ItemParentMapping
{
    public static ItemParentDto ToDto(this ProductParent itemParent)
    {
        return new ItemParentDto
        {
            Id = itemParent.Id,
            Name = itemParent.Name,
            CodeNumber = itemParent.CodeNumber,
            CodePrefix = itemParent.CodePrefix,
            ParentId = itemParent.ParentId,
            Children = itemParent.Children?.Select(x => x.ToDto())
        };
    }

    public static ProductParent ToModel(this ItemParentDto itemParentDto)
    {
        return new ProductParent
        {
            Id = itemParentDto.Id,
            Name = itemParentDto.Name,
            CodeNumber = itemParentDto.CodeNumber,
            CodePrefix = itemParentDto.CodePrefix,
            ParentId = itemParentDto.ParentId,
            Children = itemParentDto.Children?.Select(x => x.ToModel()).ToHashSet()
        };
    }
}
