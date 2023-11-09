using Retail.Core.DTOs;

namespace Retail.Application.Services.Abstraction;

public interface ICatalogItemService
{
    Task<CatalogItemDto?> GetCatalogItemAsync(Guid id);

    Task<List<CatalogItemDto>> GetCatalogItemsAsync();

    Task<CatalogItemDto> CreateCatalogItemAsync(CatalogItemDto catalogItemDto);

    Task UpdateCatalogItemAsync(CatalogItemDto catalogItemDto);

    Task DeleteCatalogItemAsync(Guid id, bool isSoftDeleting);
}
