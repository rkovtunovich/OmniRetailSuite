namespace BackOffice.Application.Mapping.Retail;

public class ReceiptItemProfile : Profile
{
    public ReceiptItemProfile()
    {
        CreateMap<ReceiptItem, ReceiptItemDto>().ReverseMap();
    }
}
