namespace RetailAssistant.Application.Mapping.ProductCatalog;

public class ProductParentProfile : Profile
{
    public ProductParentProfile()
    {
        CreateMap<ProductParent, ProductParentDto>();
        CreateMap<ProductParentDto, ProductParent>();

        CreateMap<ProductParent, AppDatabase>()
            .ConvertUsing(src => AppDatabase.ProductCatalog);
    }
}
