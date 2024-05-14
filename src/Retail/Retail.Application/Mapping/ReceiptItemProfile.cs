using Retail.Core.Entities.ReceiptAggregate;

namespace Retail.Application.Mapping;

public class ReceiptItemProfile : Profile
{
    public ReceiptItemProfile()
    {
        CreateMap<ReceiptItem, ReceiptItemDto>().ReverseMap();
    }
}
