namespace BackOffice.Application.Mapping.ProductCatalog;

public class ProductTypeProfile : Profile
{
    public ProductTypeProfile()
    {
        CreateMap<ProductType, ProductTypeDto>();
        CreateMap<ProductTypeDto, ProductType>();
    }
}
