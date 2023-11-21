using BackOffice.Application.Helpers;
using BackOffice.Application.Services.Abstraction.ProductCatalog;
using BackOffice.Core.Models.ProductCatalog;
using Microsoft.Extensions.DependencyInjection;

namespace BackOffice.Application.Services.Implementation.ProductCatalog;

public class ProductBrandService: IProductBrandService
{
    private readonly IHttpService _httpService;
    private readonly ILogger<ProductItemService> _logger;

    public ProductBrandService([FromKeyedServices(Constants.API_HTTP_CLIENT_NAME)] IHttpService httpService, ILogger<ProductItemService> logger)
    {
        _httpService = httpService;
        _logger = logger;
    }

    public event Func<ProductBrand, Task>? ProductBrandChanged;

    public async Task<List<ProductBrand>> GetBrandsAsync()
    {
        var uri = CatalogUriHelper.GetAllBrands();
        var brandDtos = await _httpService.GetAsync<List<BrandDto>>(uri);

        var brands = brandDtos?.Select(x => new ProductBrand()
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();

        return brands ?? [];
    }

    public async Task<ProductBrand> GetBrandByIdAsync(Guid id)
    {
        var uri = CatalogUriHelper.GetCatalogBrandById(id);
        var item = await _httpService.GetAsync<BrandDto>(uri);

        return item?.ToModel() ?? new();
    }

    public async Task<ProductBrand> CreateBrandAsync(ProductBrand catalogBrand)
    {
        var uri = CatalogUriHelper.CreateCatalogBrand();
        await _httpService.PostAsync(uri, catalogBrand);

        ProductBrandChanged?.Invoke(catalogBrand);

        return catalogBrand;
    }

    public async Task<ProductBrand> UpdateBrandAsync(ProductBrand catalogBrand)
    {
        var uri = CatalogUriHelper.UpdateCatalogBrand();

        await _httpService.PutAsync(uri, catalogBrand);

        ProductBrandChanged?.Invoke(catalogBrand);

        return catalogBrand;
    }

    public async Task DeleteBrandAsync(Guid id, bool useSoftDeleting)
    {
        var uri = CatalogUriHelper.DeleteCatalogBrand(id, useSoftDeleting);
        await _httpService.DeleteAsync(uri);

        ProductBrandChanged?.Invoke(new());
    }
}
