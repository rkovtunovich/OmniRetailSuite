namespace Retail.Application.Mapping;

public class StoreProfile : Profile
{
    public StoreProfile()
    {
        CreateMap<Store, StoreDto>()
            .ForMember(dest => dest.Cashiers, opt => opt.AllowNull());

        CreateMap<StoreDto, Store>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());
    }
}
