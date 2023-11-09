namespace Catalog.Application.DTOs.CatalogTDOs;

public record BrandDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public static Expression<Func<Brand, BrandDto>> Projection
    {
        get
        {
            return item => new BrandDto
            {
                Id = item.Id,
                Name = item.Name
            };
        }
    }

    public Brand ToEntity()
    {
        return new Brand()
        {
            Id = Id,
            Name = Name
        };
    }

    public static BrandDto FromEntity(Brand brand)
    {
        return new BrandDto()
        {
            Id = brand.Id,
            Name = brand.Name
        };
    }
}
