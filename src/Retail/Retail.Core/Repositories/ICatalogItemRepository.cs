using Retail.Core.Entities.ReceiptAggregate;

namespace Retail.Core.Repositories;

public interface ICatalogItemRepository
{
    Task<List<ProductItem>> GetProductItemsAsync();

    Task<ProductItem?> GetProductItemAsync(Guid productItemId);

    Task<ProductItem> AddProductItemAsync(ProductItem productItem);

    Task UpdateProductItemAsync(ProductItem productItem);

    Task DeleteProductItemAsync(Guid productItemId, bool isSoftDeleting);
}
