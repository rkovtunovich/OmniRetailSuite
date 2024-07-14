namespace RetailAssistant.Application.Mapping.Retail;

public class ReceiptItemProfile : Profile
{
    public ReceiptItemProfile()
    {
        CreateMap<ReceiptItem, ReceiptItemDto>().ReverseMap();
    }
}
