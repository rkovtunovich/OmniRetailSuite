﻿namespace BackOffice.Application.Helpers;

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

        return $"{Constants.PRODUCT_CATALOG_URI}items{filterQs}?pageIndex={page}&pageSize={take}";
    }

    public static string GetCatalogItemById(Guid id)
    {
        return $"{Constants.PRODUCT_CATALOG_URI}items/{id}";
    }

    public static string GetCatalogItemsByIds(string ids)
    {
        return $"{Constants.PRODUCT_CATALOG_URI}items/withids/{ids}";
    }

    public static string UpdateCatalogItem()
    {
        return $"{Constants.PRODUCT_CATALOG_URI}items";
    }

    public static string CreateCatalogItem()
    {
        return $"{Constants.PRODUCT_CATALOG_URI}items";
    }

    public static string DeleteCatalogItem(Guid id, bool useSoftDeleting)
    {
        return $"{Constants.PRODUCT_CATALOG_URI}items/{id}?useSoftDeleting={useSoftDeleting}";
    }

    #endregion

    #region ItemParent

    public static string GetAllItemParents()
    {
        return $"{Constants.PRODUCT_CATALOG_URI}parents";
    }

    public static string GetItemParentById(Guid id)
    {
        return $"{Constants.PRODUCT_CATALOG_URI}parents/{id}";
    }

    public static string CreateItemParent()
    {
        return $"{Constants.PRODUCT_CATALOG_URI}parents";
    }

    public static string UpdateItemParent()
    {
        return $"{Constants.PRODUCT_CATALOG_URI}parents";
    }

    public static string DeleteItemParent(Guid id, bool useSoftDeleting)
    {
        return $"{Constants.PRODUCT_CATALOG_URI}parents/{id}?useSoftDeleting={useSoftDeleting}";
    }

    #endregion

    #region CatalogType

    public static string GetAllTypes()
    {
        return $"{Constants.PRODUCT_CATALOG_URI}types";
    }

    public static string GetCatalogTypeById(Guid id)
    {
        return $"{Constants.PRODUCT_CATALOG_URI}types/{id}";
    }

    public static string CreateCatalogType()
    {
        return $"{Constants.PRODUCT_CATALOG_URI}types";
    }

    public static string UpdateCatalogType()
    {
        return $"{Constants.PRODUCT_CATALOG_URI}types";
    }

    public static string DeleteCatalogType(Guid id, bool useSoftDeleting)
    {
        return $"{Constants.PRODUCT_CATALOG_URI}types/{id}?useSoftDeleting={useSoftDeleting}";
    }

    #endregion

    #region CatalogBrand

    public static string GetAllBrands()
    {
        return $"{Constants.PRODUCT_CATALOG_URI}brands";
    }

    public static string GetCatalogBrandById(Guid id)
    {
        return $"{Constants.PRODUCT_CATALOG_URI}brands/{id}";
    }

    public static string CreateCatalogBrand()
    {
        return $"{Constants.PRODUCT_CATALOG_URI}brands";
    }

    public static string UpdateCatalogBrand()
    {
        return $"{Constants.PRODUCT_CATALOG_URI}brands";
    }

    public static string DeleteCatalogBrand(Guid id, bool isSoftDeleting)
    {
        return $"{Constants.PRODUCT_CATALOG_URI}brands/{id}?useSoftDeleting={isSoftDeleting}";
    }

    public static string GetCatalogItemsByParent(Guid parentId)
    {
        return $"{Constants.PRODUCT_CATALOG_URI}items/parent/{parentId}";
    }

    #endregion
}
