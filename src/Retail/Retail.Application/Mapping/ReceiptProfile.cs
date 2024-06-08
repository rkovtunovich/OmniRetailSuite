using Retail.Core.Entities.ReceiptAggregate;

namespace Retail.Application.Mapping;

public class ReceiptProfile : Profile
{
    public ReceiptProfile()
    {
        CreateMap<Receipt, ReceiptDto>()
            .ForMember(dest => dest.ReceiptItems, opt => opt.AllowNull());

        CreateMap<ReceiptDto, Receipt>()
            .ForMember(dest => dest.ReceiptItems, opt => opt.AllowNull())
            .ForMember(dest => dest.CashierId, opt => opt.MapFrom(src => src.Cashier.Id))
            .ForMember(dest => dest.Cashier, opt => opt.Ignore())
            .ForMember(dest => dest.StoreId, opt => opt.MapFrom(src => src.Store.Id))
            .ForMember(dest => dest.Store, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());
    }
}
