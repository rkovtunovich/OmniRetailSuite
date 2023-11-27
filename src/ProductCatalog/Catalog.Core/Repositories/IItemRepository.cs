namespace ProductCatalog.Core.Repositories;

public interface IItemRepository
{
    Task<List<Item>> GetItemsAsync(int pageSize, int pageIndex);

    Task<Item?> GetItemByIdAsync(Guid id);

    Task<List<Item>> GetItemsByIdAsync(IEnumerable<Guid> ids);

    Task<List<Item>> GetItemsByNameAsync(string name);

    Task<List<Item>> GetItemsByCategoryAsync(Guid? catalogBrandId, Guid? catalogTypeId);

    Task<List<Item>> GetItemsByParentAsync(Guid catalogParentId);

    Task<int> GetItemsCountAsync();

    Task<bool> CreateItemAsync(Item item);

    Task<bool> UpdateItemAsync(Item item);

    Task<bool> DeleteItemAsync(Guid id, bool isSoftDeleting);
}
