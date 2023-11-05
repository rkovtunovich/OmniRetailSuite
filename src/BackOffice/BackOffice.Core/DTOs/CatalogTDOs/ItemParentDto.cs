using System.Linq.Expressions;
using BackOffice.Core.Models.Catalog;

namespace BackOffice.Core.DTOs.CatalogTDOs;

public record ItemParentDto
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public ItemParentDto? Parent { get; init; }

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

    public ItemParent ToModel()
    {
        return new ItemParent()
        {
            Id = Id,
            Name = Name,
            Parent = Parent?.ToModel()
        };
    }
}
