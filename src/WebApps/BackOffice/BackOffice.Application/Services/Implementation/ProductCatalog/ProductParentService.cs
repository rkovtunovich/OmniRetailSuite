using BackOffice.Application.Helpers;
using BackOffice.Application.Mapping.ProductCatalog;
using BackOffice.Application.Services.Abstraction.ProductCatalog;
using BackOffice.Core.Models.ProductCatalog;
using Microsoft.Extensions.DependencyInjection;

namespace BackOffice.Application.Services.Implementation.ProductCatalog;

public class ProductParentService: IProductParentService
{
    private readonly IHttpService<ProductCatalogResource> _httpService;
    private readonly ILogger<ProductItemService> _logger;

    public event Func<ProductParent, Task>? ProductParentChanged;

    public ProductParentService([FromKeyedServices(Constants.PRODUCT_CATALOG_HTTP_CLIENT_NAME)] IHttpService<ProductCatalogResource> httpService, ILogger<ProductItemService> logger)
    {
        _logger = logger;
        _httpService = httpService;
    }

    public async Task<List<ProductParent>> GetItemParentsAsync()
    {
        var uri = CatalogUriHelper.GetAllItemParents();
        var itemParentDtos = await _httpService.GetAsync<List<ItemParentDto>>(uri);

        var itemParents = itemParentDtos?.Select(x => x.ToModel()).ToList();

        return itemParents ?? [];
    }

    public async Task<ProductParent> GetItemParentByIdAsync(Guid id)
    {
        var uri = CatalogUriHelper.GetItemParentById(id);
        var item = await _httpService.GetAsync<ItemParentDto>(uri);

        return item?.ToModel() ?? new();
    }

    public async Task<ProductParent> CreateItemParentAsync(ProductParent itemParent)
    {
        var uri = CatalogUriHelper.CreateItemParent();
        await _httpService.PostAsync(uri, itemParent.ToDto());

        ProductParentChanged?.Invoke(itemParent);

        return itemParent;
    }

    public async Task<ProductParent> UpdateItemParentAsync(ProductParent itemParent)
    {
        var uri = CatalogUriHelper.UpdateItemParent();

        await _httpService.PutAsync(uri, itemParent);

        ProductParentChanged?.Invoke(itemParent);

        return itemParent;
    }

    public async Task DeleteItemParentAsync(Guid id, bool useSoftDeleting)
    {
        var uri = CatalogUriHelper.DeleteItemParent(id, useSoftDeleting);
        await _httpService.DeleteAsync(uri);

        ProductParentChanged?.Invoke(new ProductParent() { Id = id });
    }
}
