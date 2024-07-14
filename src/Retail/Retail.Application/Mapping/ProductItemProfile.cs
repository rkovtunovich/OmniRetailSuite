using Retail.Core.Entities.ReceiptAggregate;

namespace Retail.Application.Mapping;

public class ProductItemProfile : Profile
{
    public ProductItemProfile()
    {
        CreateMap<ProductItem, RetailProductItemDto>();

        CreateMap<RetailProductItemDto, ProductItem>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());
    }
}
