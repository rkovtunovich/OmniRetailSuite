using Catalog.Core.Entities.CatalogAggregate;

namespace Catalog.Application.DTOs.CatalogTDOs;

public record ItemTypeDto
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public static Expression<Func<ItemType, ItemTypeDto>> Projection
    {
        get
        {
            return item => new ItemTypeDto
            {
                Id = item.Id,
                Name = item.Type
            };
        }
    }

    public ItemType ToEntity()
    {
        return new ItemType()
        {
            Id = Id,
            Type = Name
        };
    }

    public static ItemTypeDto FromItemType(ItemType itemType)
    {
        return Projection.Compile().Invoke(itemType);
    }
}
