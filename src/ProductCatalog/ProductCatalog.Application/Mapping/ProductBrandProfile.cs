namespace ProductCatalog.Application.Mapping;

public class ProductBrandProfile : Profile
{
    public ProductBrandProfile()
    {
        CreateMap<ProductBrand, ProductBrandDto>();
        CreateMap<ProductBrandDto, ProductBrand>().
            ForMember(dest => dest.CreatedAt, opt => opt.Ignore()).
            ForMember(dest => dest.UpdatedAt, opt => opt.Ignore()).
            ForMember(dest => dest.IsDeleted, opt => opt.Ignore());
    }
}
