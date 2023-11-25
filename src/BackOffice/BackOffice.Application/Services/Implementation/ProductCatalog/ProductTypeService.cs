using BackOffice.Application.Helpers;
using BackOffice.Application.Services.Abstraction.ProductCatalog;
using BackOffice.Core.Models.ProductCatalog;
using Microsoft.Extensions.DependencyInjection;

namespace BackOffice.Application.Services.Implementation.ProductCatalog;

public class ProductTypeService: IProductTypeService
{
    private readonly IHttpService _httpService;
    private readonly ILogger<ProductItemService> _logger;

    public event Func<ProductType, Task>? ProductTypeChanged;

    public ProductTypeService([FromKeyedServices(Constants.API_HTTP_CLIENT_NAME)] IHttpService httpService, ILogger<ProductItemService> logger)
    {
        _logger = logger;
        _httpService = httpService;
    }

    public async Task<List<ProductType>> GetTypesAsync()
    {
        var uri = CatalogUriHelper.GetAllTypes();
        var typeDtos = await _httpService.GetAsync<List<ItemTypeDto>>(uri);

        var types = typeDtos?.Select(x => x.ToModel()).ToList();

        return types ?? [];
    }

    public async Task<ProductType> GetTypeByIdAsync(Guid id)
    {
        var uri = CatalogUriHelper.GetCatalogTypeById(id);
        var item = await _httpService.GetAsync<ItemTypeDto>(uri);

        return item?.ToModel() ?? new();
    }

    public async Task<ProductType> CreateTypeAsync(ProductType catalogType)
    {
        var uri = CatalogUriHelper.CreateCatalogType();
        await _httpService.PostAsync(uri, catalogType);

        ProductTypeChanged?.Invoke(catalogType);

        return catalogType;
    }

    public async Task<ProductType> UpdateTypeAsync(ProductType catalogType)
    {
        var uri = CatalogUriHelper.UpdateCatalogType();

        await _httpService.PutAsync(uri, catalogType);

        ProductTypeChanged?.Invoke(catalogType);

        return catalogType;
    }

    public async Task DeleteTypeAsync(Guid id, bool useSoftDeleting)
    {
        var uri = CatalogUriHelper.DeleteCatalogType(id, useSoftDeleting);
        await _httpService.DeleteAsync(uri);

        ProductTypeChanged?.Invoke(new());
    }
}
