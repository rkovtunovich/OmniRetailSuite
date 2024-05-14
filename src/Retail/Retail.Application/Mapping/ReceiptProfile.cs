using Retail.Core.Entities.ReceiptAggregate;

namespace Retail.Application.Mapping;

public class ReceiptProfile : Profile
{
    public ReceiptProfile()
    {
        CreateMap<Receipt, ReceiptDto>().ReverseMap();
    }
}
