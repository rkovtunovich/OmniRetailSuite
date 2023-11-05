namespace Catalog.Core.Repositories;

public interface IItemParentRepository
{
    Task<bool> CreateItemParentAsync(ItemParent itemParent);

    Task<bool> DeleteItemParentAsync(int id, bool useSoftDeleting);

    Task<ItemParent?> GetItemParentAsync(int id);

    Task<List<ItemParent>> GetItemParentsAsync();

    Task<bool> UpdateItemParentAsync(ItemParent itemParent);
}
