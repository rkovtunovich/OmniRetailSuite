using BackOffice.Application.Mapping.ProductCatalog;
using BackOffice.Application.Services.Abstraction.ProductCatalog;
using BackOffice.Core.Models.ProductCatalog;
using Infrastructure.Http;
using Infrastructure.Http.ExternalResources;
using Infrastructure.Http.Uri;
using Microsoft.Extensions.DependencyInjection;

namespace BackOffice.Application.Services.Implementation.ProductCatalog;

public class ProductItemService : IProductItemService
{
    private readonly IHttpService<ProductCatalogResource> _httpService;
    private readonly ILogger<ProductItemService> _logger;

    public event Func<ProductItem, Task>? ProductItemChanged;

    public ProductItemService([FromKeyedServices(ClientNames.PRODUCT_CATALOG)] IHttpService<ProductCatalogResource> httpService, ILogger<ProductItemService> logger)
    {
        _logger = logger;
        _httpService = httpService;
    }

    public async Task<List<ProductItem>> GetItemsAsync(int page, int take, Guid? brand, Guid? type)
    {
        var uri = CatalogUriHelper.GetAll<ProductItem>(page, take);
        var paginatedItemsDto = await _httpService.GetAsync<PaginatedItemsDto>(uri);

        var items = paginatedItemsDto?.Data.Select(x => x.ToModel()).ToList() ?? [];

        return items;
    }

    public async Task<ProductItem> GetItemByIdAsync(Guid id)
    {
        var uri = CatalogUriHelper.Get<ProductItem>(id);
        var item = await _httpService.GetAsync<ItemDto>(uri);

        return item?.ToModel() ?? new();
    }

    public async Task<List<ProductItem>> GetItemsByIdsAsync(string ids)
    {
        var uri = CatalogUriHelper.GetCatalogItemsByIds(ids);

        var items = await _httpService.GetAsync<List<ItemDto>>(uri);

        return items?.Select(x => x.ToModel()).ToList() ?? [];
    }

    public async Task<List<ProductItem>> GetItemsByParent(Guid parentId)
    {
        var uri = CatalogUriHelper.GetCatalogItemsByParent(parentId);

        var items = await _httpService.GetAsync<PaginatedItemsDto>(uri);

        return items?.Data.Select(x => x.ToModel()).ToList() ?? [];
    }

    public async Task<ProductItem> UpdateItemAsync(ProductItem catalogItem)
    {
        var uri = CatalogUriHelper.Update<ProductItem>();

        await _httpService.PutAsync(uri, catalogItem.ToDto());

        ProductItemChanged?.Invoke(catalogItem);

        return catalogItem;
    }

    public async Task<ProductItem> CreateItemAsync(ProductItem catalogItem)
    {
        var uri = CatalogUriHelper.Add<ProductItem>();
        await _httpService.PostAsync(uri, catalogItem.ToDto());

        ProductItemChanged?.Invoke(catalogItem);

        return catalogItem;
    }

    public async Task DeleteItemAsync(Guid id, bool useSoftDeleting)
    {
        var uri = CatalogUriHelper.Delete<ProductItem>(id, useSoftDeleting);
        await _httpService.DeleteAsync(uri);

        ProductItemChanged?.Invoke(new ProductItem() { Id = id });
    }
}
