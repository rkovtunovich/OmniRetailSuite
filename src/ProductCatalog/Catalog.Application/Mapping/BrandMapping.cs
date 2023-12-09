namespace ProductCatalog.Application.Mapping;

public static class BrandMapping
{
    public static BrandDto ToDto(this Brand catalogBrand)
    {
        return new BrandDto
        {
            Id = catalogBrand.Id,
            Name = catalogBrand.Name,
            CodeNumber = catalogBrand.CodeNumber,
            CodePrefix = catalogBrand.CodePrefix
        };
    }

    public static Brand ToEntity(this BrandDto catalogBrand)
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
