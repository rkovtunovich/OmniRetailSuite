﻿namespace BackOffice.Application.Mapping.ProductCatalog;

public class ProductParentProfile : Profile
{
    public ProductParentProfile()
    {
        CreateMap<ProductParent, ProductParentDto>();

        CreateMap<ProductParentDto, ProductParent>().
            ForMember(dest => dest.Parent, opt => opt.Ignore());
    }
}
