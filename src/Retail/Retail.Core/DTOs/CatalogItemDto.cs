using Retail.Core.Entities.ReceiptAggregate;

namespace Retail.Core.DTOs;

public record CatalogItemDto
{
    public int Id { get; init; }

    public string Name { get; init; } = null!;

    public static CatalogItemDto FromCatalogItem(CatalogItem catalogItem)
    {
        return new CatalogItemDto
        {
            Id = catalogItem.Id,
            Name = catalogItem.Name,
        };
    }

    public static List<CatalogItemDto> FromCatalogItems(List<CatalogItem> catalogItems)
    {
        return catalogItems.Select(FromCatalogItem).ToList();
    }

    public CatalogItem ToCatalogItem()
    {
        return new CatalogItem
        {
            Id = Id,
            Name = Name,
        };
    }
}
