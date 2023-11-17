using Retail.Core.Entities.ReceiptAggregate;

namespace Retail.Core.DTOs;

public record CatalogItemDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = null!;

    public static CatalogItemDto FromProductItem(ProductItem catalogItem)
    {
        return new CatalogItemDto
        {
            Id = catalogItem.Id,
            Name = catalogItem.Name,
        };
    }

    public static List<CatalogItemDto> FromCatalogItems(List<ProductItem> catalogItems)
    {
        return catalogItems.Select(FromProductItem).ToList();
    }

    public ProductItem ToProductItem()
    {
        return new ProductItem
        {
            Id = Id,
            Name = Name,
        };
    }
}
