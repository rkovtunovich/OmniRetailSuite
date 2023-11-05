using Retail.Core.DTOs;

namespace Retail.Application.Services.Abstraction;

public interface ICatalogItemService
{
    Task<CatalogItemDto?> GetCatalogItemAsync(int id);

    Task<List<CatalogItemDto>> GetCatalogItemsAsync();

    Task<CatalogItemDto> CreateCatalogItemAsync(CatalogItemDto catalogItemDto);

    Task UpdateCatalogItemAsync(CatalogItemDto catalogItemDto);

    Task DeleteCatalogItemAsync(int id, bool isSoftDeleting);
}
