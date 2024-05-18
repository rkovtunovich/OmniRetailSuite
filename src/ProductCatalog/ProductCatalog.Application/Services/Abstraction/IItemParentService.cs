namespace ProductCatalog.Application.Services.Abstraction;

public interface IItemParentService
{
    Task<List<ProductParentDto>> GetItemParentsAsync();

    Task<ProductParentDto> GetItemParentByIdAsync(Guid id);

    Task<ProductParentDto> CreateItemParentAsync(ProductParentDto itemParent);

    Task<ProductParentDto> UpdateItemParentAsync(ProductParentDto itemParent);

    Task DeleteItemParentAsync(Guid id, bool isSoftDeleting);
}
