using BackOffice.Application.Mapping.ProductCatalog;
using BackOffice.Application.Services.Abstraction.ProductCatalog;
using BackOffice.Core.Models.ProductCatalog;
using Infrastructure.Http;
using Infrastructure.Http.ExternalResources;
using Infrastructure.Http.Uri;
using Microsoft.Extensions.DependencyInjection;

namespace BackOffice.Application.Services.Implementation.ProductCatalog;

public class ProductBrandService: IProductBrandService
{
    private readonly IHttpService<ProductCatalogResource> _httpService;
    private readonly ILogger<ProductItemService> _logger;

    public ProductBrandService([FromKeyedServices(ClientNames.PRODUCT_CATALOG)] IHttpService<ProductCatalogResource> httpService, ILogger<ProductItemService> logger)
    {
        _httpService = httpService;
        _logger = logger;
    }

    public event Func<ProductBrand, Task>? ProductBrandChanged;

    public async Task<List<ProductBrand>> GetBrandsAsync()
    {
        var uri = CatalogUriHelper.GetAll<ProductBrand>();
        var brandDtos = await _httpService.GetAsync<List<BrandDto>>(uri);

        var brands = brandDtos?.Select(x => x.ToModel()).ToList();

        return brands ?? [];
    }

    public async Task<ProductBrand> GetBrandByIdAsync(Guid id)
    {
        var uri = CatalogUriHelper.Get<ProductBrand>(id);
        var item = await _httpService.GetAsync<BrandDto>(uri);

        return item?.ToModel() ?? new();
    }

    public async Task<ProductBrand> CreateBrandAsync(ProductBrand catalogBrand)
    {
        var uri = CatalogUriHelper.Add<ProductBrand>();
        await _httpService.PostAsync(uri, catalogBrand);

        ProductBrandChanged?.Invoke(catalogBrand);

        return catalogBrand;
    }

    public async Task<ProductBrand> UpdateBrandAsync(ProductBrand catalogBrand)
    {
        var uri = CatalogUriHelper.Update<ProductBrand>();

        await _httpService.PutAsync(uri, catalogBrand);

        ProductBrandChanged?.Invoke(catalogBrand);

        return catalogBrand;
    }

    public async Task DeleteBrandAsync(Guid id, bool useSoftDeleting)
    {
        var uri = CatalogUriHelper.Delete<ProductBrand>(id, useSoftDeleting);
        await _httpService.DeleteAsync(uri);

        ProductBrandChanged?.Invoke(new());
    }
}
