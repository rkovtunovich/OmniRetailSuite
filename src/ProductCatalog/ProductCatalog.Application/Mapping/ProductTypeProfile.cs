namespace ProductCatalog.Application.Mapping;

public class ProductTypeProfile : Profile
{
    public ProductTypeProfile()
    {
        CreateMap<ProductType, ProductTypeDto>();
        CreateMap<ProductTypeDto, ProductType>().
            ForMember(dest => dest.CreatedAt, opt => opt.Ignore()).
            ForMember(dest => dest.UpdatedAt, opt => opt.Ignore()).
            ForMember(dest => dest.IsDeleted, opt => opt.Ignore());
    }
}
