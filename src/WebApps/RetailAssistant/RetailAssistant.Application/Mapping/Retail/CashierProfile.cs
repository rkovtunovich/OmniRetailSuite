namespace RetailAssistant.Application.Mapping.Retail;

public class CashierProfile : Profile
{
    public CashierProfile()
    {
        CreateMap<Cashier, CashierDto>();
        CreateMap<CashierDto, Cashier>();

        CreateMap<Cashier, AppDatabase>()
            .ConvertUsing(src => AppDatabase.Retail);
    }
}
