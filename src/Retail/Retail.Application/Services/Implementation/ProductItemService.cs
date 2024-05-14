using Retail.Core.Entities.ReceiptAggregate;
using Retail.Core.Repositories;

namespace Retail.Application.Services.Implementation;

public class ProductItemService(ICatalogItemRepository productItemRepository, IMapper mapper, ILogger<ProductItemService> logger) : IProductItemService
{
    public async Task<ProductItemDto?> GetProductItemAsync(Guid id)
    {
        try
        {
            var productItem = await productItemRepository.GetProductItemAsync(id);

            return mapper.Map<ProductItemDto>(productItem);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error while getting productItem: id {id}");
            throw;
        }
    }

    public async Task<List<ProductItemDto>> GetProductItemsAsync()
    {
        try
        {
            var productItems = await productItemRepository.GetProductItemsAsync();
            return mapper.Map<List<ProductItemDto>>(productItems);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while getting productItems");
            throw;
        }
    }

    public async Task<ProductItemDto> CreateProductItemAsync(ProductItemDto ProductItemDto)
    {
        try
        {
            var productItem = mapper.Map<ProductItem>(ProductItemDto);
            await productItemRepository.AddProductItemAsync(productItem);
            return mapper.Map<ProductItemDto>(productItem);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error whole creating productItem");
            throw;
        }
    }

    public async Task UpdateProductItemAsync(ProductItemDto productItemDto)
    {
        try
        {
            var productItem = await productItemRepository.GetProductItemAsync(productItemDto.Id) 
                ?? throw new Exception($"ProductItem with id {productItemDto.Id} not found");

            productItem.Name = productItemDto.Name;

            await productItemRepository.UpdateProductItemAsync(productItem);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error while updating productItem: id {productItemDto.Id}");
        }
    }

    public async Task DeleteProductItemAsync(Guid id, bool isSoftDeleting)
    {
        try
        {
            await productItemRepository.DeleteProductItemAsync(id, isSoftDeleting);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error while deleting productItem: id {id}");
        }
    }
}
