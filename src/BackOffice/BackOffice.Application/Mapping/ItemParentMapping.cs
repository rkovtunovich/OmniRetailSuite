using BackOffice.Core.Models.Product;

namespace BackOffice.Application.Mapping;

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

    public static ItemParent ToModel(this ItemParentDto itemParentDto)
    {
        return new ItemParent
        {
            Id = itemParentDto.Id,
            Name = itemParentDto.Name
        };
    }
}
