namespace Retail.Application.Services.Abstraction;

public interface IProductItemService
{
    Task<ProductItemDto?> GetProductItemAsync(Guid id);

    Task<List<ProductItemDto>> GetProductItemsAsync();

    Task<ProductItemDto> CreateProductItemAsync(ProductItemDto catalogItemDto);

    Task UpdateProductItemAsync(ProductItemDto catalogItemDto);

    Task DeleteProductItemAsync(Guid id, bool isSoftDeleting);
}
