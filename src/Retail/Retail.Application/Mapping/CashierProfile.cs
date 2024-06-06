namespace Retail.Application.Mapping;

public class CashierProfile : Profile
{
    public CashierProfile()
    {
        CreateMap<Cashier, CashierDto>();
        
        CreateMap<CashierDto, Cashier>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());
    }
}
