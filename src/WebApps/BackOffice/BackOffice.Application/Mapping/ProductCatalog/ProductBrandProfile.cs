namespace BackOffice.Application.Mapping.ProductCatalog;

public class ProductBrandProfile : Profile
{
    public ProductBrandProfile()
    {
        CreateMap<ProductBrand, ProductBrandDto>();
        CreateMap<ProductBrandDto, ProductBrand>();
    }
}
