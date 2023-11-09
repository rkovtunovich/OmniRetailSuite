using Retail.Core.Repositories;

namespace Retail.Application.Services.Implementation;

public class CatalogItemService : ICatalogItemService
{
    private readonly ICatalogItemRepository _catalogItemRepository;
    private readonly ILogger<CatalogItemService> _logger;

    public CatalogItemService(ICatalogItemRepository catalogItemRepository, ILogger<CatalogItemService> logger)
    {
        _catalogItemRepository = catalogItemRepository;
        _logger = logger;
    }

    public async Task<CatalogItemDto?> GetCatalogItemAsync(Guid id)
    {
        try
        {
            var catalogItem = await _catalogItemRepository.GetCatalogItemAsync(id);

            return catalogItem is null ? null : CatalogItemDto.FromCatalogItem(catalogItem);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while getting catalogItem: id {id}");
            throw;
        }
    }

    public async Task<List<CatalogItemDto>> GetCatalogItemsAsync()
    {
        try
        {
            var catalogItems = await _catalogItemRepository.GetCatalogItemsAsync();
            return catalogItems.Select(catalogItem => CatalogItemDto.FromCatalogItem(catalogItem)).ToList();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting catalogItems");
            throw;
        }
    }

    public async Task<CatalogItemDto> CreateCatalogItemAsync(CatalogItemDto catalogItemDto)
    {
        try
        {
            var catalogItem = catalogItemDto.ToCatalogItem();
            catalogItem.CreatedAt = DateTime.UtcNow;
            await _catalogItemRepository.AddCatalogItemAsync(catalogItem);
            return CatalogItemDto.FromCatalogItem(catalogItem);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error whole creating catalogItem");
            throw;
        }
    }

    public async Task UpdateCatalogItemAsync(CatalogItemDto catalogItemDto)
    {
        try
        {
            var catalogItem = await _catalogItemRepository.GetCatalogItemAsync(catalogItemDto.Id) ?? throw new Exception($"CatalogItem with id {catalogItemDto.Id} not found");
            catalogItem.Name = catalogItemDto.Name;
            catalogItem.UpdatedAt = DateTime.UtcNow;

            await _catalogItemRepository.UpdateCatalogItemAsync(catalogItem);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while updating catalogItem: id {catalogItemDto.Id}");
        }
    }

    public async Task DeleteCatalogItemAsync(Guid id, bool isSoftDeleting)
    {
        try
        {
            await _catalogItemRepository.DeleteCatalogItemAsync(id, isSoftDeleting);

        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while deleting catalogItem: id {id}");
        }
    }
}
