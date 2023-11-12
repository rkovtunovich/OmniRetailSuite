namespace ProductCatalog.Application.Services.Abstraction;

public interface IItemParentService
{
    Task<List<ItemParentDto>> GetItemParentsAsync();

    Task<ItemParentDto> GetItemParentByIdAsync(Guid id);

    Task<ItemParentDto> CreateItemParentAsync(ItemParentDto itemParent);

    Task<ItemParentDto> UpdateItemParentAsync(ItemParentDto itemParent);

    Task DeleteItemParentAsync(Guid id, bool isSoftDeleting);
}
