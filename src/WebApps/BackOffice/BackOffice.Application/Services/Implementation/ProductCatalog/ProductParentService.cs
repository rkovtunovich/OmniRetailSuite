using BackOffice.Application.Mapping.ProductCatalog;
using BackOffice.Application.Services.Abstraction.ProductCatalog;
using BackOffice.Core.Models.ProductCatalog;
using Microsoft.Extensions.DependencyInjection;

namespace BackOffice.Application.Services.Implementation.ProductCatalog;

public class ProductParentService: IProductParentService
{
    private readonly IHttpService<ProductCatalogClientSettings> _httpService;
    private readonly ILogger<ProductItemService> _logger;
    private readonly ProductCatalogUriResolver _productCatalogUriResolver;

    public event Func<ProductParent, Task>? ProductParentChanged;

    public ProductParentService([FromKeyedServices(ClientNames.PRODUCT_CATALOG)] IHttpService<ProductCatalogClientSettings> httpService, ILogger<ProductItemService> logger, ProductCatalogUriResolver productCatalogUriResolver)
    {
        _logger = logger;
        _httpService = httpService;
        _productCatalogUriResolver = productCatalogUriResolver;
    }

    public async Task<List<ProductParent>> GetItemParentsAsync()
    {
        var uri = _productCatalogUriResolver.GetAll<ProductParent>();
        var itemParentDtos = await _httpService.GetAsync<List<ItemParentDto>>(uri);

        var itemParents = itemParentDtos?.Select(x => x.ToModel()).ToList();

        return itemParents ?? [];
    }

    public async Task<ProductParent> GetItemParentByIdAsync(Guid id)
    {
        var uri = _productCatalogUriResolver.Get<ProductParent>(id);
        var item = await _httpService.GetAsync<ItemParentDto>(uri);

        return item?.ToModel() ?? new();
    }

    public async Task<ProductParent> CreateItemParentAsync(ProductParent itemParent)
    {
        var uri = _productCatalogUriResolver.Add<ProductParent>();
        await _httpService.PostAsync(uri, itemParent.ToDto());

        ProductParentChanged?.Invoke(itemParent);

        return itemParent;
    }

    public async Task<ProductParent> UpdateItemParentAsync(ProductParent itemParent)
    {
        var uri = _productCatalogUriResolver.Update<ProductParent>();

        await _httpService.PutAsync(uri, itemParent);

        ProductParentChanged?.Invoke(itemParent);

        return itemParent;
    }

    public async Task DeleteItemParentAsync(Guid id, bool useSoftDeleting)
    {
        var uri = _productCatalogUriResolver.Delete<ProductParent>(id, useSoftDeleting);
        await _httpService.DeleteAsync(uri);

        ProductParentChanged?.Invoke(new ProductParent() { Id = id });
    }
}
