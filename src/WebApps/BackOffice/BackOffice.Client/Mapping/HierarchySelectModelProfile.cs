using AutoMapper;
using BackOffice.Core.Models.ProductCatalog;

namespace BackOffice.Client.Mapping;

public class HierarchySelectModelProfile : Profile
{
    public HierarchySelectModelProfile()
    {
        CreateMap<ProductParent, HierarchySelectModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.Children));
    }
}
