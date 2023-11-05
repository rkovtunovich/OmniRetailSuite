using Catalog.Core.Entities.CatalogAggregate;

namespace Catalog.Application.DTOs.CatalogTDOs;

public record ItemParentDto
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public ItemParentDto? Parent { get; init; }

    public ItemParent ToEntity()
    {
        return new ItemParent
        {
            Id = Id,
            Name = Name,
            Parent = Parent?.ToEntity()
        };
    }

    public static Expression<Func<ItemParent, ItemParentDto>> Projection
    {
        get
        {
            return item => new ItemParentDto
            {
                Id = item.Id,
                Name = item.Name,
                Parent = item.Parent == null ? null : Projection.Compile().Invoke(item.Parent)
            };
        }
    }

    public static ItemParentDto FromItemParent(ItemParent itemParent)
    {
        return Projection.Compile().Invoke(itemParent);
    }
}
