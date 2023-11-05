using BackOffice.Core.Models.Catalog;
using System.Linq.Expressions;

namespace BackOffice.Core.DTOs.CatalogTDOs;

public record CatalogTypeDto
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public static Expression<Func<CatalogType, CatalogTypeDto>> Projection
    {
        get
        {
            return item => new CatalogTypeDto
            {
                Id = item.Id,
                Name = item.Name
            };
        }
    }

    public CatalogType ToModel()
    {
        return new CatalogType()
        {
            Id = Id,
            Name = Name
        };
    }
}
