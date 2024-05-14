namespace Retail.Application.Mapping;

public class CashierProfile : Profile
{
    public CashierProfile()
    {
        CreateMap<Cashier, CashierDto>();
        CreateMap<CashierDto, Cashier>();
    }
}
