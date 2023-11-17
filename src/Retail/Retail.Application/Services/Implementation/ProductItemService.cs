using Retail.Core.Repositories;

namespace Retail.Application.Services.Implementation;

public class ProductItemService : IProductItemService
{
    private readonly ICatalogItemRepository _productItemRepository;
    private readonly ILogger<ProductItemService> _logger;

    public ProductItemService(ICatalogItemRepository productItemRepository, ILogger<ProductItemService> logger)
    {
        _productItemRepository = productItemRepository;
        _logger = logger;
    }

    public async Task<CatalogItemDto?> GetProductItemAsync(Guid id)
    {
        try
        {
            var productItem = await _productItemRepository.GetProductItemAsync(id);

            return productItem is null ? null : CatalogItemDto.FromProductItem(productItem);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while getting productItem: id {id}");
            throw;
        }
    }

    public async Task<List<CatalogItemDto>> GetProductItemsAsync()
    {
        try
        {
            var productItems = await _productItemRepository.GetProductItemsAsync();
            return productItems.Select(productItem => CatalogItemDto.FromProductItem(productItem)).ToList();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting productItems");
            throw;
        }
    }

    public async Task<CatalogItemDto> CreateProductItemAsync(CatalogItemDto ProductItemDto)
    {
        try
        {
            var productItem = ProductItemDto.ToProductItem();
            await _productItemRepository.AddProductItemAsync(productItem);
            return CatalogItemDto.FromProductItem(productItem);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error whole creating productItem");
            throw;
        }
    }

    public async Task UpdateProductItemAsync(CatalogItemDto productItemDto)
    {
        try
        {
            var productItem = await _productItemRepository.GetProductItemAsync(productItemDto.Id) ?? throw new Exception($"ProductItem with id {productItemDto.Id} not found");
            productItem.Name = productItemDto.Name;

            await _productItemRepository.UpdateProductItemAsync(productItem);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while updating productItem: id {productItemDto.Id}");
        }
    }

    public async Task DeleteProductItemAsync(Guid id, bool isSoftDeleting)
    {
        try
        {
            await _productItemRepository.DeleteProductItemAsync(id, isSoftDeleting);

        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while deleting productItem: id {id}");
        }
    }
}
