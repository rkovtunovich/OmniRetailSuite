namespace ProductCatalog.Core.Repositories;

public interface IItemRepository
{
    Task<List<ProductItem>> GetItemsAsync(int pageSize, int pageIndex);

    Task<ProductItem?> GetItemByIdAsync(Guid id);

    Task<List<ProductItem>> GetItemsByIdAsync(IEnumerable<Guid> ids);

    Task<List<ProductItem>> GetItemsByNameAsync(string name);

    Task<List<ProductItem>> GetItemsByCategoryAsync(Guid? catalogBrandId, Guid? catalogTypeId);

    Task<List<ProductItem>> GetItemsByParentAsync(Guid catalogParentId);

    Task<int> GetItemsCountAsync();

    Task<bool> CreateItemAsync(ProductItem item);

    Task<bool> UpdateItemAsync(ProductItem item);

    Task<bool> DeleteItemAsync(Guid id, bool isSoftDeleting);
}
