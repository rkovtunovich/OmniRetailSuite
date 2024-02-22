namespace RetailAssistant.Application.Mapping.ProductCatalog;

public class CatalogProductItemProfile : Profile
{
    public CatalogProductItemProfile()
    {
        CreateMap<CatalogProductItem, Contracts.Dtos.ProductCatalog.ProductItemDto>();
        CreateMap<Contracts.Dtos.ProductCatalog.ProductItemDto, CatalogProductItem>();
    }
}
