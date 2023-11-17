using Retail.Core.DTOs;

namespace Retail.Application.Services.Abstraction;

public interface IProductItemService
{
    Task<CatalogItemDto?> GetProductItemAsync(Guid id);

    Task<List<CatalogItemDto>> GetProductItemsAsync();

    Task<CatalogItemDto> CreateProductItemAsync(CatalogItemDto catalogItemDto);

    Task UpdateProductItemAsync(CatalogItemDto catalogItemDto);

    Task DeleteProductItemAsync(Guid id, bool isSoftDeleting);
}
