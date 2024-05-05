namespace BackOffice.Application.Mapping.ProductCatalog;

public class ProductItemProfile : Profile
{
    public ProductItemProfile()
    {
        CreateMap<ProductItem, Contracts.Dtos.ProductCatalog.ProductItemDto>();

        CreateMap<Contracts.Dtos.ProductCatalog.ProductItemDto, ProductItem>()
            .ForMember(x => x.PictureName, opt => opt.Ignore());
    }
}
