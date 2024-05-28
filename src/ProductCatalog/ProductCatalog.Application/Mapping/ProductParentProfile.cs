namespace ProductCatalog.Application.Mapping;

public class ProductParentProfile : Profile
{
    public ProductParentProfile()
    {
        CreateMap<ProductParent, ProductParentDto>();

        CreateMap<ProductParentDto, ProductParent>().
            ForMember(dest => dest.Parent, opt => opt.Ignore()).
            ForMember(dest => dest.Children, opt => opt.Ignore()).
            ForMember(dest => dest.CreatedAt, opt => opt.Ignore()).
            ForMember(dest => dest.UpdatedAt, opt => opt.Ignore()).
            ForMember(dest => dest.IsDeleted, opt => opt.Ignore());
    }
}
