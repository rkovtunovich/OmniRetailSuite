using Retail.Core.Entities.ReceiptAggregate;

namespace Retail.Core.Repositories;

public interface ICatalogItemRepository
{
    Task<List<CatalogItem>> GetCatalogItemsAsync();

    Task<CatalogItem?> GetCatalogItemAsync(Guid receiptItemId);

    Task<CatalogItem> AddCatalogItemAsync(CatalogItem catalogItem);

    Task UpdateCatalogItemAsync(CatalogItem receiptItem);

    Task DeleteCatalogItemAsync(Guid catalogItemId, bool useSoftDeleting);
}
