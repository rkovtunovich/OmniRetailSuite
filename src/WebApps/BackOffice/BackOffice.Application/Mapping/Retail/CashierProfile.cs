namespace BackOffice.Application.Mapping.Retail;

public class CashierProfile: Profile
{
    public CashierProfile()
    {
        CreateMap<Cashier, CashierDto>();
        CreateMap<CashierDto, Cashier>();
    }
}
