using Constants;

namespace UriHelper;

public static class CatalogUriHelper
{
    #region CatalogItem

    public static string GetAllCatalogItems(int page, int take, Guid? brand, Guid? type)
    {
        string? filterQs;
        if (type.HasValue)
        {
            var brandQs = brand.HasValue ? brand.Value.ToString() : string.Empty;
            filterQs = $"/type/{type.Value}/brand/{brandQs}";

        }
        else if (brand.HasValue)
        {
            var brandQs = brand.HasValue ? brand.Value.ToString() : string.Empty;
            filterQs = $"/type/all/brand/{brandQs}";
        }
        else
        {
            filterQs = string.Empty;
        }

        return $"{ResourceBasePaths.PRODUCT_CATALOG}items{filterQs}?pageIndex={page}&pageSize={take}";
    }

    public static string GetCatalogItemById(Guid id)
    {
        return $"{ResourceBasePaths.PRODUCT_CATALOG}items/{id}";
    }

    public static string GetCatalogItemsByIds(string ids)
    {
        return $"{ResourceBasePaths.PRODUCT_CATALOG}items/withids/{ids}";
    }

    public static string UpdateCatalogItem()
    {
        return $"{ResourceBasePaths.PRODUCT_CATALOG}items";
    }

    public static string CreateCatalogItem()
    {
        return $"{ResourceBasePaths.PRODUCT_CATALOG}items";
    }

    public static string DeleteCatalogItem(Guid id, bool useSoftDeleting)
    {
        return $"{ResourceBasePaths.PRODUCT_CATALOG}items/{id}?useSoftDeleting={useSoftDeleting}";
    }

    #endregion

    #region ItemParent

    public static string GetAllItemParents()
    {
        return $"{ResourceBasePaths.PRODUCT_CATALOG}parents";
    }

    public static string GetItemParentById(Guid id)
    {
        return $"{ResourceBasePaths.PRODUCT_CATALOG}parents/{id}";
    }

    public static string CreateItemParent()
    {
        return $"{ResourceBasePaths.PRODUCT_CATALOG}parents";
    }

    public static string UpdateItemParent()
    {
        return $"{ResourceBasePaths.PRODUCT_CATALOG}parents";
    }

    public static string DeleteItemParent(Guid id, bool useSoftDeleting)
    {
        return $"{ResourceBasePaths.PRODUCT_CATALOG}parents/{id}?useSoftDeleting={useSoftDeleting}";
    }

    #endregion

    #region CatalogType

    public static string GetAllTypes()
    {
        return $"{ResourceBasePaths.PRODUCT_CATALOG}types";
    }

    public static string GetCatalogTypeById(Guid id)
    {
        return $"{ResourceBasePaths.PRODUCT_CATALOG}types/{id}";
    }

    public static string CreateCatalogType()
    {
        return $"{ResourceBasePaths.PRODUCT_CATALOG}types";
    }

    public static string UpdateCatalogType()
    {
        return $"{ResourceBasePaths.PRODUCT_CATALOG}types";
    }

    public static string DeleteCatalogType(Guid id, bool useSoftDeleting)
    {
        return $"{ResourceBasePaths.PRODUCT_CATALOG}types/{id}?useSoftDeleting={useSoftDeleting}";
    }

    #endregion

    #region CatalogBrand

    public static string GetAllBrands()
    {
        return $"{ResourceBasePaths.PRODUCT_CATALOG}brands";
    }

    public static string GetCatalogBrandById(Guid id)
    {
        return $"{ResourceBasePaths.PRODUCT_CATALOG}brands/{id}";
    }

    public static string CreateCatalogBrand()
    {
        return $"{ResourceBasePaths.PRODUCT_CATALOG}brands";
    }

    public static string UpdateCatalogBrand()
    {
        return $"{ResourceBasePaths.PRODUCT_CATALOG}brands";
    }

    public static string DeleteCatalogBrand(Guid id, bool isSoftDeleting)
    {
        return $"{ResourceBasePaths.PRODUCT_CATALOG}brands/{id}?useSoftDeleting={isSoftDeleting}";
    }

    public static string GetCatalogItemsByParent(Guid parentId)
    {
        return $"{ResourceBasePaths.PRODUCT_CATALOG}items/parent/{parentId}";
    }

    #endregion
}
