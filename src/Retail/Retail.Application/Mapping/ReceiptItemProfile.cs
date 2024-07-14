using Retail.Core.Entities.ReceiptAggregate;

namespace Retail.Application.Mapping;

public class ReceiptItemProfile : Profile
{
    public ReceiptItemProfile()
    {
        CreateMap<ReceiptItem, ReceiptItemDto>();

        CreateMap<ReceiptItemDto, ReceiptItem>()
            .ForMember(dest => dest.Receipt, opt => opt.Ignore())
            .ForMember(dest => dest.ProductItemId, opt => opt.MapFrom(src => src.ProductItem.Id))
            .ForMember(dest => dest.ProductItem, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());
    }
}
