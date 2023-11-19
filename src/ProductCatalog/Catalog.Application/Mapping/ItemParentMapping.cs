using ProductCatalog.Core.Entities.ProductAggregate;

namespace ProductCatalog.Application.Mapping;

public static class ItemParentMapping
{
    public static ItemParentDto ToDto(this ItemParent itemParent)
    {
        return new ItemParentDto
        {
            Id = itemParent.Id,
            Name = itemParent.Name,
            Parent = itemParent.Parent == null ? null : ToDto(itemParent.Parent),
            Children = itemParent.Children?.Select(x => ToDto(x))
        };
    }

    public static ItemParent ToEntity(this ItemParentDto itemParentDto)
    {
        return new ItemParent
        {
            Id = itemParentDto.Id,
            Name = itemParentDto.Name,
            Parent = itemParentDto.Parent?.ToEntity()
        };
    }
}
