﻿namespace RetailAssistant.Application.Mapping.ProductCatalog;

public class CatalogProductItemProfile : Profile
{
    public CatalogProductItemProfile()
    {
        CreateMap<CatalogProductItem, ProductItemDto>();
        CreateMap<ProductItemDto, CatalogProductItem>();

        CreateMap<CatalogProductItem, AppDatabase>()
            .ConvertUsing(src => AppDatabase.ProductCatalog);
    }
}
