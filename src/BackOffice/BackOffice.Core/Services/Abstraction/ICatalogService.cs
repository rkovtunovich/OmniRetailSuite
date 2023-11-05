using BackOffice.Core.Models.Catalog;

namespace BackOffice.Core.Services.Abstraction;

public interface ICatalogService
{
    #region Events

    event Func<CatalogItem, Task> CatalogItemChanged;

    event Func<CatalogBrand, Task> CatalogBrandChanged;

    event Func<CatalogType, Task> CatalogTypeChanged;

    event Func<ItemParent, Task> ItemParentChanged;

    #endregion

    #region CatalogItem

    Task<List<CatalogItem>> GetItemsAsync(int page, int take, int? brand = null, int? type = null);

    Task<CatalogItem> GetItemByIdAsync(int id);

    Task<List<CatalogItem>> GetItemsByIdsAsync(string ids);

    Task<CatalogItem> CreateItemAsync(CatalogItem catalogItem);

    Task<CatalogItem> UpdateItemAsync(CatalogItem catalogItem);

    Task DeleteItemAsync(int id, bool useSoftDeleting);

    #endregion

    #region ItemParent

    Task<List<ItemParent>> GetItemParentsAsync();

    Task<ItemParent> GetItemParentByIdAsync(int id);

    Task<ItemParent> CreateItemParentAsync(ItemParent itemParent);

    Task<ItemParent> UpdateItemParentAsync(ItemParent itemParent);

    Task DeleteItemParentAsync(int id, bool useSoftDeleting);

    #endregion

    #region CatalogBrand

    Task<List<CatalogBrand>> GetBrandsAsync();

    Task<CatalogBrand> GetBrandByIdAsync(int id);

    Task<CatalogBrand> CreateBrandAsync(CatalogBrand catalogBrand);

    Task<CatalogBrand> UpdateBrandAsync(CatalogBrand catalogBrand);

    Task DeleteBrandAsync(int id, bool useSoftDeleting);

    #endregion

    #region CatalogType

    Task<List<CatalogType>> GetTypesAsync();

    Task<CatalogType> GetTypeByIdAsync(int id);

    Task<CatalogType> CreateTypeAsync(CatalogType catalogType);

    Task<CatalogType> UpdateTypeAsync(CatalogType catalogType);

    Task DeleteTypeAsync(int id, bool useSoftDeleting);

    #endregion
}
