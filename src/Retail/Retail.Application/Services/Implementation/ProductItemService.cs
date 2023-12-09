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

    public async Task<ProductItemDto?> GetProductItemAsync(Guid id)
    {
        try
        {
            var productItem = await _productItemRepository.GetProductItemAsync(id);

            return productItem?.ToDto();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while getting productItem: id {id}");
            throw;
        }
    }

    public async Task<List<ProductItemDto>> GetProductItemsAsync()
    {
        try
        {
            var productItems = await _productItemRepository.GetProductItemsAsync();
            return productItems.Select(productItem => productItem.ToDto()).ToList();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting productItems");
            throw;
        }
    }

    public async Task<ProductItemDto> CreateProductItemAsync(ProductItemDto ProductItemDto)
    {
        try
        {
            var productItem = ProductItemDto.ToEntity();
            await _productItemRepository.AddProductItemAsync(productItem);
            return productItem.ToDto();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error whole creating productItem");
            throw;
        }
    }

    public async Task UpdateProductItemAsync(ProductItemDto productItemDto)
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
