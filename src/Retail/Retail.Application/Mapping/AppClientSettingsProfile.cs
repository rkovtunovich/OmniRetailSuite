namespace Retail.Application.Mapping;

public class AppClientSettingsProfile : Profile
{
    public AppClientSettingsProfile()
    {
        CreateMap<AppClientSettings, AppClientSettingsDto>();

        CreateMap<AppClientSettingsDto, AppClientSettings>()
            .ForMember(dest => dest.Store, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());
    }
}
