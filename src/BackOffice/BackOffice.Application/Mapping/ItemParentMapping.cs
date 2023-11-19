using BackOffice.Core.Models.Product;

namespace BackOffice.Application.Mapping;

public static class ItemParentMapping
{
    public static ItemParentDto ToDto(this ItemParent itemParent)
    {
        return new ItemParentDto
        {
            Id = itemParent.Id,
            Name = itemParent.Name,
            Parent = itemParent.Parent?.ToDto(),
            Children = itemParent.Children?.Select(x => x.ToDto())
        };
    }

    public static ItemParent ToModel(this ItemParentDto itemParentDto)
    {
        return new ItemParent
        {
            Id = itemParentDto.Id,
            Name = itemParentDto.Name,
            Parent = itemParentDto.Parent?.ToModel(),
            ParentId = itemParentDto.Parent?.Id,
            Children = itemParentDto.Children?.Select(x => x.ToModel()).ToHashSet()
        };
    }
}
