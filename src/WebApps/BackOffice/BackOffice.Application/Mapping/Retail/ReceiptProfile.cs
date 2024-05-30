namespace BackOffice.Application.Mapping.Retail;

public class ReceiptProfile : Profile
{
    public ReceiptProfile()
    {
        CreateMap<Receipt, ReceiptDto>();

        CreateMap<ReceiptDto, Receipt>().
            ForMember(dest => dest.Name, opt => opt.MapFrom(src => string.Empty));
    }
}
