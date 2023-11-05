namespace Catalog.Core.Repositories;

public interface IItemRepository
{
    Task<List<Item>> GetItemsAsync(int pageSize, int pageIndex);

    Task<Item?> GetItemByIdAsync(int id);

    Task<List<Item>> GetItemsByIdAsync(IEnumerable<int> ids);

    Task<List<Item>> GetItemsByNameAsync(string name);

    Task<List<Item>> GetItemsByCategoryAsync(int? catalogBrandId, int? catalogTypeId);

    Task<int> GetItemsCountAsync();

    Task<bool> CreateItemAsync(Item item);

    Task<bool> UpdateItemAsync(Item item);

    Task<bool> DeleteItemAsync(int id, bool useSoftDeleting);
}
