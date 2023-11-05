using BackOffice.Core.DTOs.CatalogTDOs;
using BackOffice.Core.Models.Catalog;

namespace BackOffice.Client.Services.Implementation;

public class CatalogService : ICatalogService
{
    #region Fields

    private readonly IHttpService _httpService;
    private readonly ILogger<CatalogService> _logger;

    #endregion

    #region Events

    public event Func<CatalogItem, Task>? CatalogItemChanged;
    public event Func<CatalogBrand, Task>? CatalogBrandChanged;
    public event Func<CatalogType, Task>? CatalogTypeChanged;
    public event Func<ItemParent, Task>? ItemParentChanged;

    #endregion

    #region Ctor

    public CatalogService([FromKeyedServices(Constants.API_HTTP_CLIENT_NAME)] IHttpService httpService, ILogger<CatalogService> logger)
    {
        _logger = logger;
        _httpService = httpService;
    }

    #endregion

    #region CatalogItem

    public async Task<List<CatalogItem>> GetItemsAsync(int page, int take, int? brand, int? type)
    {
        var uri = CatalogUriHelper.GetAllCatalogItems(page, take, brand, type);
        var paginatedItemsDto = await _httpService.GetAsync<PaginatedItemsDto>(uri);

        var items = paginatedItemsDto?.Data.Select(x => new CatalogItem()
        {
            Id = x.Id,
            CatalogBrand = x.CatalogBrand.ToModel(),
            CatalogBrandId = x.CatalogBrandId,
            CatalogType = x.CatalogType?.ToModel(),
            CatalogTypeId = x.CatalogTypeId,
            Description = x.Description,
            Name = x.Name,
            PictureUri = x.PictureUri,
            Price = x.Price,
            Barcode = x.Barcode,
        }).ToList() ?? new();

        return items;
    }

    public async Task<CatalogItem> GetItemByIdAsync(int id)
    {
        var uri = CatalogUriHelper.GetCatalogItemById(id);
        var item = await _httpService.GetAsync<CatalogItemDto>(uri);

        return item?.ToModel() ?? new();
    }

    public async Task<List<CatalogItem>> GetItemsByIdsAsync(string ids)
    {
        var uri = CatalogUriHelper.GetCatalogItemsByIds(ids);

        var items = await _httpService.GetAsync<List<CatalogItemDto>>(uri);

        return items?.Select(x => x.ToModel()).ToList() ?? new();
    }

    public async Task<CatalogItem> UpdateItemAsync(CatalogItem catalogItem)
    {
        var uri = CatalogUriHelper.UpdateCatalogItem();

        await _httpService.PutAsync(uri, catalogItem);

        CatalogItemChanged?.Invoke(catalogItem);

        return catalogItem;
    }

    public async Task<CatalogItem> CreateItemAsync(CatalogItem catalogItem)
    {
        var uri = CatalogUriHelper.CreateCatalogItem();
        await _httpService.PostAsync(uri, catalogItem);

        CatalogItemChanged?.Invoke(catalogItem);

        return catalogItem;
    }

    public async Task DeleteItemAsync(int id, bool useSoftDeleting)
    {
        var uri = CatalogUriHelper.DeleteCatalogItem(id, useSoftDeleting);
        await _httpService.DeleteAsync(uri);

        CatalogItemChanged?.Invoke(new CatalogItem() { Id = id });
    }

    #endregion

    #region ItemParent

    public async Task<List<ItemParent>> GetItemParentsAsync()
    {
        var uri = CatalogUriHelper.GetAllItemParents();
        var itemParentDtos = await _httpService.GetAsync<List<ItemParentDto>>(uri);

        var itemParents = itemParentDtos?.Select(x => new ItemParent()
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();

        return itemParents ?? new();
    }

    public async Task<ItemParent> GetItemParentByIdAsync(int id)
    {
        var uri = CatalogUriHelper.GetItemParentById(id);
        var item = await _httpService.GetAsync<ItemParentDto>(uri);

        return item?.ToModel() ?? new();
    }

    public async Task<ItemParent> CreateItemParentAsync(ItemParent itemParent)
    {
        var uri = CatalogUriHelper.CreateItemParent();
        await _httpService.PostAsync(uri, itemParent);

        ItemParentChanged?.Invoke(itemParent);

        return itemParent;
    }

    public async Task<ItemParent> UpdateItemParentAsync(ItemParent itemParent)
    {
        var uri = CatalogUriHelper.UpdateItemParent();

        await _httpService.PutAsync(uri, itemParent);

        ItemParentChanged?.Invoke(itemParent);

        return itemParent;
    }

    public async Task DeleteItemParentAsync(int id, bool useSoftDeleting)
    {
        var uri = CatalogUriHelper.DeleteItemParent(id, useSoftDeleting);
        await _httpService.DeleteAsync(uri);

        ItemParentChanged?.Invoke(new ItemParent() { Id = id });
    }

    #endregion

    #region CatalogBrand

    public async Task<List<CatalogBrand>> GetBrandsAsync()
    {
        var uri = CatalogUriHelper.GetAllBrands();
        var brandDtos = await _httpService.GetAsync<List<CatalogBrandDto>>(uri);

        var brands = brandDtos?.Select(x => new CatalogBrand()
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();

        return brands ?? new();
    }

    public async Task<CatalogBrand> GetBrandByIdAsync(int id)
    {
        var uri = CatalogUriHelper.GetCatalogBrandById(id);
        var item = await _httpService.GetAsync<CatalogBrandDto>(uri);

        return item?.ToModel() ?? new();
    }

    public async Task<CatalogBrand> CreateBrandAsync(CatalogBrand catalogBrand)
    {
        var uri = CatalogUriHelper.CreateCatalogBrand();
        await _httpService.PostAsync(uri, catalogBrand);

        CatalogBrandChanged?.Invoke(catalogBrand);

        return catalogBrand;
    }

    public async Task<CatalogBrand> UpdateBrandAsync(CatalogBrand catalogBrand)
    {
        var uri = CatalogUriHelper.UpdateCatalogBrand();

        await _httpService.PutAsync(uri, catalogBrand);

        CatalogBrandChanged?.Invoke(catalogBrand);

        return catalogBrand;
    }

    public async Task DeleteBrandAsync(int id, bool useSoftDeleting)
    {
        var uri = CatalogUriHelper.DeleteCatalogBrand(id, useSoftDeleting);
        await _httpService.DeleteAsync(uri);

        CatalogBrandChanged?.Invoke(new());
    }

    #endregion

    #region CatalogType

    public async Task<List<CatalogType>> GetTypesAsync()
    {
        var uri = CatalogUriHelper.GetAllTypes();
        var typeDtos = await _httpService.GetAsync<List<CatalogTypeDto>>(uri);

        var types = typeDtos?.Select(x => new CatalogType()
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();

        return types ?? new();
    }

    public async Task<CatalogType> GetTypeByIdAsync(int id)
    {
        var uri = CatalogUriHelper.GetCatalogTypeById(id);
        var item = await _httpService.GetAsync<CatalogTypeDto>(uri);

        return item?.ToModel() ?? new();
    }

    public async Task<CatalogType> CreateTypeAsync(CatalogType catalogType)
    {
        var uri = CatalogUriHelper.CreateCatalogType();
        await _httpService.PostAsync(uri, catalogType);

        CatalogTypeChanged?.Invoke(catalogType);

        return catalogType;
    }

    public async Task<CatalogType> UpdateTypeAsync(CatalogType catalogType)
    {
        var uri = CatalogUriHelper.UpdateCatalogType();

        await _httpService.PutAsync(uri, catalogType);

        CatalogTypeChanged?.Invoke(catalogType);

        return catalogType;
    }

    public async Task DeleteTypeAsync(int id, bool useSoftDeleting)
    {
        var uri = CatalogUriHelper.DeleteCatalogType(id, useSoftDeleting);
        await _httpService.DeleteAsync(uri);

        CatalogTypeChanged?.Invoke(new());
    }

    #endregion
}
