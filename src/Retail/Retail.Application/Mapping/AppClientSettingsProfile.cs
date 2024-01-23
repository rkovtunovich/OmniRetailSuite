namespace Retail.Application.Mapping;

internal class AppClientSettingsProfile : Profile
{
    public AppClientSettingsProfile()
    {
        CreateMap<AppClientSettings, AppClientSettingsDto>();
        CreateMap<AppClientSettingsDto, AppClientSettings>();
    }
}
