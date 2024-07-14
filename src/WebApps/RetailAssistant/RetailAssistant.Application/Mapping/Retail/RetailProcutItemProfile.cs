namespace RetailAssistant.Application.Mapping.Retail;

public class RetailProcutItemProfile : Profile
{
    public RetailProcutItemProfile()
    {
        CreateMap<RetailProductItem, RetailProductItemDto>();
        CreateMap<RetailProductItemDto, RetailProductItem>();
    }
}
