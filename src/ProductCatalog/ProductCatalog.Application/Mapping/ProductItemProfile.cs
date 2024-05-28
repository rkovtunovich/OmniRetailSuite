namespace ProductCatalog.Application.Mapping;

public class ProductItemProfile : Profile
{
    public ProductItemProfile()
    {
        CreateMap<ProductItem, ProductItemDto>();

        CreateMap<ProductItemDto, ProductItem>().
            ForMember(dest => dest.ProductBrandId, opt => opt.MapFrom(src => src.ProductBrand == null ? Guid.Empty : src.ProductBrand.Id)).
            ForMember(dest => dest.ProductTypeId, opt => opt.MapFrom(src => src.ProductType == null ? Guid.Empty : src.ProductType.Id)).
            ForMember(dest => dest.ProductBrand, opt => opt.Ignore()).
            ForMember(dest => dest.ProductType, opt => opt.Ignore()).
            ForMember(dest => dest.Parent, opt => opt.Ignore()).
            ForMember(dest => dest.CreatedAt, opt => opt.Ignore()).
            ForMember(dest => dest.UpdatedAt, opt => opt.Ignore()).
            ForMember(dest => dest.IsDeleted, opt => opt.Ignore());
    }
}
