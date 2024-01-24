using RetailAssistant.Client;

namespace RetailAssistant.Client.Helpers;

public static class CatalogUriHelper
{
    #region CatalogItem

    public static string GetAllCatalogItems(int page, int take, int? brand, int? type)
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

        return $"{Constants.CATALOG_BASE_URI}items{filterQs}?pageIndex={page}&pageSize={take}";
    }

    public static string GetCatalogItemById(int id)
    {
        return $"{Constants.CATALOG_BASE_URI}items/{id}";
    }

    public static string GetCatalogItemsByIds(string ids)
    {
        return $"{Constants.CATALOG_BASE_URI}items/withids/{ids}";
    }

    public static string UpdateCatalogItem()
    {
        return $"{Constants.CATALOG_BASE_URI}items";
    }

    public static string CreateCatalogItem()
    {
        return $"{Constants.CATALOG_BASE_URI}items";
    }

    public static string DeleteCatalogItem(int id, bool useSoftDeleting)
    {
        return $"{Constants.CATALOG_BASE_URI}items/{id}?useSoftDeleting={useSoftDeleting}";
    }

    #endregion

    #region ItemParent

    public static string GetAllItemParents()
    {
        return $"{Constants.CATALOG_BASE_URI}parents";
    }

    public static string GetItemParentById(int id)
    {
        return $"{Constants.CATALOG_BASE_URI}parents/{id}";
    }

    public static string CreateItemParent()
    {
        return $"{Constants.CATALOG_BASE_URI}parents";
    }

    public static string UpdateItemParent()
    {
        return $"{Constants.CATALOG_BASE_URI}parents";
    }

    public static string DeleteItemParent(int id, bool useSoftDeleting)
    {
        return $"{Constants.CATALOG_BASE_URI}parents/{id}?useSoftDeleting={useSoftDeleting}";
    }

    #endregion

    #region CatalogType

    public static string GetAllTypes()
    {
        return $"{Constants.CATALOG_BASE_URI}types";
    }

    public static string GetCatalogTypeById(int id)
    {
        return $"{Constants.CATALOG_BASE_URI}types/{id}";
    }

    public static string CreateCatalogType()
    {
        return $"{Constants.CATALOG_BASE_URI}types";
    }

    public static string UpdateCatalogType()
    {
        return $"{Constants.CATALOG_BASE_URI}types";
    }

    public static string DeleteCatalogType(int id, bool useSoftDeleting)
    {
        return $"{Constants.CATALOG_BASE_URI}types/{id}?useSoftDeleting={useSoftDeleting}";
    }

    #endregion

    #region CatalogBrand

    public static string GetAllBrands()
    {
        return $"{Constants.CATALOG_BASE_URI}brands";
    }

    public static string GetCatalogBrandById(int id)
    {
        return $"{Constants.CATALOG_BASE_URI}brands/{id}";
    }

    public static string CreateCatalogBrand()
    {
        return $"{Constants.CATALOG_BASE_URI}brands";
    }

    public static string UpdateCatalogBrand()
    {
        return $"{Constants.CATALOG_BASE_URI}brands";
    }

    public static string DeleteCatalogBrand(int id, bool useSoftDeleting)
    {
        return $"{Constants.CATALOG_BASE_URI}brands/{id}?useSoftDeleting={useSoftDeleting}";
    }

    #endregion
}
