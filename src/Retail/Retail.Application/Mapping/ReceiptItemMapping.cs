using Retail.Core.Entities.ReceiptAggregate;

namespace Retail.Application.Mapping;

public static class ReceiptItemMapping
{
    public static ReceiptItemDto ToDto(this ReceiptItem receiptItem)
    {
        return new ReceiptItemDto
        {
            Id = receiptItem.Id,
            ProductItem = receiptItem.ProductItem.ToDto(),
            Quantity = receiptItem.Quantity,
            LineNumber = receiptItem.LineNumber,
            ReceiptId = receiptItem.ReceiptId,
            TotalPrice = receiptItem.TotalPrice,
            UnitPrice = receiptItem.UnitPrice                 
        };
    }

    public static ReceiptItem ToEntity(this ReceiptItemDto receiptItemDto)
    {
        return new ReceiptItem
        {
            Id = receiptItemDto.Id,
            ProductItem = receiptItemDto.ProductItem.ToEntity(),
            Quantity = receiptItemDto.Quantity,
            LineNumber = receiptItemDto.LineNumber,
            ReceiptId = receiptItemDto.ReceiptId,
            TotalPrice = receiptItemDto.TotalPrice,
            UnitPrice = receiptItemDto.UnitPrice
        };
    }
}
