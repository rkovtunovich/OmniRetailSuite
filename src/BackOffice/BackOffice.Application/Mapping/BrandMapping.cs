using BackOffice.Core.Models.ProductCatalog;

namespace BackOffice.Application.Mapping;

public static class BrandMapping
{
    public static BrandDto? ToDto(this ProductBrand catalogBrand)
    {
        return new BrandDto
        {
            Id = catalogBrand.Id,
            Name = catalogBrand.Name,
            CodeNumber = catalogBrand.CodeNumber,
            CodePrefix = catalogBrand.CodePrefix
        };
    }

    public static ProductBrand ToModel(this BrandDto catalogBrand)
    {
        return new ProductBrand
        {
            Id = catalogBrand.Id,
            Name = catalogBrand.Name,
            CodeNumber = catalogBrand.CodeNumber,
            CodePrefix = catalogBrand.CodePrefix
        };
    }
}
