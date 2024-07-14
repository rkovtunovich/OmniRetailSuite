namespace BackOffice.Application.Mapping.Retail;

public class RetailProductItemProfile : Profile
{
    public RetailProductItemProfile()
    {
        CreateMap<RetailProductItem, RetailProductItemDto>();

        CreateMap<RetailProductItemDto, RetailProductItem>();
    }
}
