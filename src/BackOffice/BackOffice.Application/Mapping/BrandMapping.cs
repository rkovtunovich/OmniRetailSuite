using BackOffice.Core.Models.Product;

namespace BackOffice.Application.Mapping;

public static class BrandMapping
{
    public static BrandDto? ToDto(this Brand catalogBrand)
    {
        return new BrandDto
        {
            Id = catalogBrand.Id,
            Name = catalogBrand.Name
        };
    }

    public static Brand ToModel(this BrandDto catalogBrand)
    {
        return new Brand
        {
            Id = catalogBrand.Id,
            Name = catalogBrand.Name
        };
    }
}
