using AutoMapper;

namespace Retail.Application.Mapping;

public class StoreProfile : Profile
{
    public StoreProfile()
    {
        CreateMap<Store, StoreDto>();
        CreateMap<StoreDto, Store>();
    }
}
