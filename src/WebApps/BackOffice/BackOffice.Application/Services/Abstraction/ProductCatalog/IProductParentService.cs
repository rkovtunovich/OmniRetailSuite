using BackOffice.Core.Models.ProductCatalog;

namespace BackOffice.Application.Services.Abstraction.ProductCatalog;

public interface IProductParentService
{
    event Func<ProductParent, Task> ProductParentChanged;

    Task<List<ProductParent>> GetItemParentsAsync();

    Task<ProductParent> GetItemParentByIdAsync(Guid id);

    Task<ProductParent> CreateItemParentAsync(ProductParent itemParent);

    Task<ProductParent> UpdateItemParentAsync(ProductParent itemParent);

    Task DeleteItemParentAsync(Guid id, bool useSoftDeleting);
}
