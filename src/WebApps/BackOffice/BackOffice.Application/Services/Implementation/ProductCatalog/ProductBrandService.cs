using BackOffice.Application.Mapping.ProductCatalog;
using BackOffice.Application.Services.Abstraction.ProductCatalog;
using BackOffice.Core.Models.ProductCatalog;
using Microsoft.Extensions.DependencyInjection;

namespace BackOffice.Application.Services.Implementation.ProductCatalog;

public class ProductBrandService: IProductBrandService
{
    private readonly IHttpService<ProductCatalogClientSettings> _httpService;
    private readonly ILogger<ProductItemService> _logger;
    private readonly ProductCatalogUriResolver _productCatalogUriResolver;

    public ProductBrandService([FromKeyedServices(ClientNames.PRODUCT_CATALOG)] IHttpService<ProductCatalogClientSettings> httpService, ILogger<ProductItemService> logger, ProductCatalogUriResolver productCatalogUriResolver)
    {
        _httpService = httpService;
        _logger = logger;
        _productCatalogUriResolver = productCatalogUriResolver;
    }

    public event Func<ProductBrand, Task>? ProductBrandChanged;

    public async Task<List<ProductBrand>> GetBrandsAsync()
    {
        var uri = _productCatalogUriResolver.GetAll<ProductBrand>();
        var brandDtos = await _httpService.GetAsync<List<BrandDto>>(uri);

        var brands = brandDtos?.Select(x => x.ToModel()).ToList();

        return brands ?? [];
    }

    public async Task<ProductBrand> GetBrandByIdAsync(Guid id)
    {
        var uri = _productCatalogUriResolver.Get<ProductBrand>(id);
        var item = await _httpService.GetAsync<BrandDto>(uri);

        return item?.ToModel() ?? new();
    }

    public async Task<ProductBrand> CreateBrandAsync(ProductBrand catalogBrand)
    {
        var uri = _productCatalogUriResolver.Add<ProductBrand>();
        await _httpService.PostAsync(uri, catalogBrand);

        ProductBrandChanged?.Invoke(catalogBrand);

        return catalogBrand;
    }

    public async Task<ProductBrand> UpdateBrandAsync(ProductBrand catalogBrand)
    {
        var uri = _productCatalogUriResolver.Update<ProductBrand>();

        await _httpService.PutAsync(uri, catalogBrand);

        ProductBrandChanged?.Invoke(catalogBrand);

        return catalogBrand;
    }

    public async Task DeleteBrandAsync(Guid id, bool useSoftDeleting)
    {
        var uri = _productCatalogUriResolver.Delete<ProductBrand>(id, useSoftDeleting);
        await _httpService.DeleteAsync(uri);

        ProductBrandChanged?.Invoke(new());
    }
}
