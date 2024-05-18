namespace ProductCatalog.Application.Mapping;

public static class BrandMapping
{
    public static ProductBrandDto ToDto(this Brand catalogBrand)
    {
        return new ProductBrandDto
        {
            Id = catalogBrand.Id,
            Name = catalogBrand.Name,
            CodeNumber = catalogBrand.CodeNumber,
            CodePrefix = catalogBrand.CodePrefix
        };
    }

    public static Brand ToEntity(this ProductBrandDto catalogBrand)
    {
        return new Brand
        {
            Id = catalogBrand.Id,
            Name = catalogBrand.Name,
            CodeNumber = catalogBrand.CodeNumber,
            CodePrefix = catalogBrand.CodePrefix
        };
    }
}
