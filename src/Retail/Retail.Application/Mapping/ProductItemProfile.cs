using Retail.Core.Entities.ReceiptAggregate;

namespace Retail.Application.Mapping;

public class ProductItemProfile : Profile
{
    public ProductItemProfile()
    {
        CreateMap<ProductItem, ProductItemDto>().ReverseMap();
    }
}
