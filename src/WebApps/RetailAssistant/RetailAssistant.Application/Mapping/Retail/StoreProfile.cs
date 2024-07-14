using AutoMapper;

namespace RetailAssistant.Application.Mapping.Retail;

public class StoreProfile : Profile
{
    public StoreProfile()
    {
        CreateMap<Store, StoreDto>();
        CreateMap<StoreDto, Store>();
    }
}
