using Retail.Core.Entities.ReceiptAggregate;

namespace Retail.Application.Mapping;

public static class ReceiptMapping
{
    public static Receipt ToEntity(this ReceiptDto dto)
    {
        return new Receipt
        {
            Id = dto.Id,
            Date = dto.Date,
            CodeNumber = dto.CodeNumber,
            CodePrefix = dto.CodePrefix,
            CashierId = dto.CashierId,
            StoreId = dto.StoreId,
            TotalPrice = dto.TotalPrice,
            ReceiptItems = dto.ReceiptItems.Select(x => x.ToEntity()).ToList()
        };
    }

    public static ReceiptDto ToDto(this Receipt entity)
    {
        return new ReceiptDto
        {
            Id = entity.Id,
            Date = entity.Date,
            CodeNumber = entity.CodeNumber,
            CodePrefix = entity.CodePrefix,
            CashierId = entity.CashierId,
            StoreId = entity.StoreId,
            TotalPrice = entity.TotalPrice,
            ReceiptItems = entity.ReceiptItems.Select(x => x.ToDto()).ToList()
        };
    }
}
