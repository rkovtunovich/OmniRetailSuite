using BackOffice.Core.Models.Product;

namespace BackOffice.Application.Services.Abstraction;

public interface IProductCatalogService
{
    #region Events

    event Func<Item, Task> CatalogItemChanged;

    event Func<Brand, Task> CatalogBrandChanged;

    event Func<ItemType, Task> CatalogTypeChanged;

    event Func<ItemParent, Task> ItemParentChanged;

    #endregion

    #region CatalogItem

    Task<List<Item>> GetItemsAsync(int page, int take, Guid? brand = null, Guid? type = null);

    Task<Item> GetItemByIdAsync(Guid id);

    Task<List<Item>> GetItemsByIdsAsync(string ids);

    Task<Item> CreateItemAsync(Item catalogItem);

    Task<Item> UpdateItemAsync(Item catalogItem);

    Task DeleteItemAsync(Guid id, bool useSoftDeleting);

    #endregion

    #region ItemParent

    Task<List<ItemParent>> GetItemParentsAsync();

    Task<ItemParent> GetItemParentByIdAsync(Guid id);

    Task<ItemParent> CreateItemParentAsync(ItemParent itemParent);

    Task<ItemParent> UpdateItemParentAsync(ItemParent itemParent);

    Task DeleteItemParentAsync(Guid id, bool useSoftDeleting);

    #endregion

    #region CatalogBrand

    Task<List<Brand>> GetBrandsAsync();

    Task<Brand> GetBrandByIdAsync(Guid id);

    Task<Brand> CreateBrandAsync(Brand catalogBrand);

    Task<Brand> UpdateBrandAsync(Brand catalogBrand);

    Task DeleteBrandAsync(Guid id, bool useSoftDeleting);

    #endregion

    #region CatalogType

    Task<List<ItemType>> GetTypesAsync();

    Task<ItemType> GetTypeByIdAsync(Guid id);

    Task<ItemType> CreateTypeAsync(ItemType catalogType);

    Task<ItemType> UpdateTypeAsync(ItemType catalogType);

    Task DeleteTypeAsync(Guid id, bool useSoftDeleting);

    #endregion
}
