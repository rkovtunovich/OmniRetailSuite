namespace Catalog.Application.Services.Abstraction;

public interface IItemParentService
{
    Task<List<ItemParent>> GetItemParentsAsync();

    Task<ItemParent> GetItemParentByIdAsync(int id);

    Task<ItemParent> CreateItemParentAsync(ItemParent itemParent);

    Task<ItemParent> UpdateItemParentAsync(ItemParent itemParent);

    Task DeleteItemParentAsync(int id, bool isSoftDeleting);
}
