using BackOffice.Core.Models.ProductCatalog;

namespace BackOffice.Application.Services.Abstraction.ProductCatalog;

public interface IProductItemService
{
    event Func<ProductItem, Task> ProductItemChanged;

    Task<List<ProductItem>> GetItemsAsync(int page, int take, Guid? brand = null, Guid? type = null);

    Task<ProductItem> GetItemByIdAsync(Guid id);

    Task<List<ProductItem>> GetItemsByIdsAsync(string ids);

    Task<List<ProductItem>> GetItemsByParent(Guid parentId);

    Task<ProductItem> CreateItemAsync(ProductItem catalogItem);

    Task<ProductItem> UpdateItemAsync(ProductItem catalogItem);

    Task DeleteItemAsync(Guid id, bool useSoftDeleting);
}
