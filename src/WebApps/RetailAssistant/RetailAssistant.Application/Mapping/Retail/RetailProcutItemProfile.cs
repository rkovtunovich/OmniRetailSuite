namespace RetailAssistant.Application.Mapping.Retail;

public class RetailProcutItemProfile : Profile
{
    public RetailProcutItemProfile()
    {
        CreateMap<RetailProductItem, Contracts.Dtos.Retail.ProductItemDto>();
        CreateMap<Contracts.Dtos.Retail.ProductItemDto, RetailProductItem>();
    }
}
