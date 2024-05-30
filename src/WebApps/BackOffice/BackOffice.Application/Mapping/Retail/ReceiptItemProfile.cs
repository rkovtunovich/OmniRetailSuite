namespace BackOffice.Application.Mapping.Retail;

public class ReceiptItemProfile : Profile
{
    public ReceiptItemProfile()
    {
        CreateMap<ReceiptItem, ReceiptItemDto>();

        CreateMap<ReceiptItemDto, ReceiptItem>().
            ForMember(dest => dest.Receipt, opt => opt.Ignore());
    }
}
