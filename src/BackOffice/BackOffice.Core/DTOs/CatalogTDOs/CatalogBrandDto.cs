using BackOffice.Core.Models.Catalog;
using System.Linq.Expressions;

namespace BackOffice.Core.DTOs.CatalogTDOs;

public record CatalogBrandDto
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public static Expression<Func<CatalogBrand, CatalogBrandDto>> Projection
    {
        get
        {
            return item => new CatalogBrandDto
            {
                Id = item.Id,
                Name = item.Name
            };
        }
    }

    public CatalogBrand ToModel()
    {
        return new CatalogBrand()
        {
            Id = Id,
            Name = Name
        };
    }
}
