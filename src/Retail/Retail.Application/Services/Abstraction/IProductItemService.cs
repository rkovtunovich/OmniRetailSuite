namespace Retail.Application.Services.Abstraction;

public interface IProductItemService
{
    Task<RetailProductItemDto?> GetProductItemAsync(Guid id);

    Task<List<RetailProductItemDto>> GetProductItemsAsync();

    Task<RetailProductItemDto> CreateProductItemAsync(RetailProductItemDto catalogItemDto);

    Task UpdateProductItemAsync(RetailProductItemDto catalogItemDto);

    Task DeleteProductItemAsync(Guid id, bool isSoftDeleting);
}
