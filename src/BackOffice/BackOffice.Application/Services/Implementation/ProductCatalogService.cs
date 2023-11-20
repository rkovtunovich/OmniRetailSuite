using BackOffice.Application.Helpers;
using BackOffice.Core.Models.Product;
using Microsoft.Extensions.DependencyInjection;

namespace BackOffice.Application.Services.Implementation;

public class ProductCatalogService : IProductCatalogService
{
    #region Fields

    private readonly IHttpService _httpService;
    private readonly ILogger<ProductCatalogService> _logger;

    #endregion

    #region Events

    public event Func<Item, Task>? CatalogItemChanged;
    public event Func<Brand, Task>? CatalogBrandChanged;
    public event Func<ItemType, Task>? CatalogTypeChanged;
    public event Func<ItemParent, Task>? ItemParentChanged;

    #endregion

    #region Ctor

    public ProductCatalogService([FromKeyedServices(Constants.API_HTTP_CLIENT_NAME)] IHttpService httpService, ILogger<ProductCatalogService> logger)
    {
        _logger = logger;
        _httpService = httpService;
    }

    #endregion

    #region Item

    public async Task<List<Item>> GetItemsAsync(int page, int take, Guid? brand, Guid? type)
    {
        var uri = CatalogUriHelper.GetAllCatalogItems(page, take, brand, type);
        var paginatedItemsDto = await _httpService.GetAsync<PaginatedItemsDto>(uri);

        var items = paginatedItemsDto?.Data.Select(x => x.ToModel()).ToList() ?? [];

        return items;
    }

    public async Task<Item> GetItemByIdAsync(Guid id)
    {
        var uri = CatalogUriHelper.GetCatalogItemById(id);
        var item = await _httpService.GetAsync<ItemDto>(uri);

        return item?.ToModel() ?? new();
    }

    public async Task<List<Item>> GetItemsByIdsAsync(string ids)
    {
        var uri = CatalogUriHelper.GetCatalogItemsByIds(ids);

        var items = await _httpService.GetAsync<List<ItemDto>>(uri);

        return items?.Select(x => x.ToModel()).ToList() ?? [];
    }

    public async Task<Item> UpdateItemAsync(Item catalogItem)
    {
        var uri = CatalogUriHelper.UpdateCatalogItem();

        await _httpService.PutAsync(uri, catalogItem);

        CatalogItemChanged?.Invoke(catalogItem);

        return catalogItem;
    }

    public async Task<Item> CreateItemAsync(Item catalogItem)
    {
        var uri = CatalogUriHelper.CreateCatalogItem();
        await _httpService.PostAsync(uri, catalogItem.ToDto());

        CatalogItemChanged?.Invoke(catalogItem);

        return catalogItem;
    }

    public async Task DeleteItemAsync(Guid id, bool useSoftDeleting)
    {
        var uri = CatalogUriHelper.DeleteCatalogItem(id, useSoftDeleting);
        await _httpService.DeleteAsync(uri);

        CatalogItemChanged?.Invoke(new Item() { Id = id });
    }

    #endregion

    #region ItemParent

    public async Task<List<ItemParent>> GetItemParentsAsync()
    {
        var uri = CatalogUriHelper.GetAllItemParents();
        var itemParentDtos = await _httpService.GetAsync<List<ItemParentDto>>(uri);

        var itemParents = itemParentDtos?.Select(x => x.ToModel()).ToList();

        return itemParents ?? [];
    }

    public async Task<ItemParent> GetItemParentByIdAsync(Guid id)
    {
        var uri = CatalogUriHelper.GetItemParentById(id);
        var item = await _httpService.GetAsync<ItemParentDto>(uri);

        return item?.ToModel() ?? new();
    }

    public async Task<ItemParent> CreateItemParentAsync(ItemParent itemParent)
    {
        var uri = CatalogUriHelper.CreateItemParent();
        await _httpService.PostAsync(uri, itemParent.ToDto());

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

    public async Task DeleteItemParentAsync(Guid id, bool useSoftDeleting)
    {
        var uri = CatalogUriHelper.DeleteItemParent(id, useSoftDeleting);
        await _httpService.DeleteAsync(uri);

        ItemParentChanged?.Invoke(new ItemParent() { Id = id });
    }

    #endregion

    #region Brand

    public async Task<List<Brand>> GetBrandsAsync()
    {
        var uri = CatalogUriHelper.GetAllBrands();
        var brandDtos = await _httpService.GetAsync<List<BrandDto>>(uri);

        var brands = brandDtos?.Select(x => new Brand()
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();

        return brands ?? [];
    }

    public async Task<Brand> GetBrandByIdAsync(Guid id)
    {
        var uri = CatalogUriHelper.GetCatalogBrandById(id);
        var item = await _httpService.GetAsync<BrandDto>(uri);

        return item?.ToModel() ?? new();
    }

    public async Task<Brand> CreateBrandAsync(Brand catalogBrand)
    {
        var uri = CatalogUriHelper.CreateCatalogBrand();
        await _httpService.PostAsync(uri, catalogBrand);

        CatalogBrandChanged?.Invoke(catalogBrand);

        return catalogBrand;
    }

    public async Task<Brand> UpdateBrandAsync(Brand catalogBrand)
    {
        var uri = CatalogUriHelper.UpdateCatalogBrand();

        await _httpService.PutAsync(uri, catalogBrand);

        CatalogBrandChanged?.Invoke(catalogBrand);

        return catalogBrand;
    }

    public async Task DeleteBrandAsync(Guid id, bool useSoftDeleting)
    {
        var uri = CatalogUriHelper.DeleteCatalogBrand(id, useSoftDeleting);
        await _httpService.DeleteAsync(uri);

        CatalogBrandChanged?.Invoke(new());
    }

    #endregion

    #region ItemType

    public async Task<List<ItemType>> GetTypesAsync()
    {
        var uri = CatalogUriHelper.GetAllTypes();
        var typeDtos = await _httpService.GetAsync<List<ItemTypeDto>>(uri);

        var types = typeDtos?.Select(x => new ItemType()
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();

        return types ?? [];
    }

    public async Task<ItemType> GetTypeByIdAsync(Guid id)
    {
        var uri = CatalogUriHelper.GetCatalogTypeById(id);
        var item = await _httpService.GetAsync<ItemTypeDto>(uri);

        return item?.ToModel() ?? new();
    }

    public async Task<ItemType> CreateTypeAsync(ItemType catalogType)
    {
        var uri = CatalogUriHelper.CreateCatalogType();
        await _httpService.PostAsync(uri, catalogType);

        CatalogTypeChanged?.Invoke(catalogType);

        return catalogType;
    }

    public async Task<ItemType> UpdateTypeAsync(ItemType catalogType)
    {
        var uri = CatalogUriHelper.UpdateCatalogType();

        await _httpService.PutAsync(uri, catalogType);

        CatalogTypeChanged?.Invoke(catalogType);

        return catalogType;
    }

    public async Task DeleteTypeAsync(Guid id, bool useSoftDeleting)
    {
        var uri = CatalogUriHelper.DeleteCatalogType(id, useSoftDeleting);
        await _httpService.DeleteAsync(uri);

        CatalogTypeChanged?.Invoke(new());
    }

    #endregion
}
