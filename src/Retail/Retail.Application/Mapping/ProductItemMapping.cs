using Retail.Core.Entities.ReceiptAggregate;

namespace Retail.Application.Mapping;

public static class ProductItemMapping
{
    public static ProductItemDto ToDto(this ProductItem productItem)
    {
        return new ProductItemDto
        {
            Id = productItem.Id,
            Name = productItem.Name,
            CodeNumber = productItem.CodeNumber,
            CodePrefix = productItem.CodePrefix
        };
    }

    public static ProductItem ToEntity(this ProductItemDto productItem)
    {
        return new ProductItem
        {
            Id = productItem.Id,
            Name = productItem.Name,
            CodeNumber = productItem.CodeNumber,
            CodePrefix = productItem.CodePrefix
        };
    }
}
