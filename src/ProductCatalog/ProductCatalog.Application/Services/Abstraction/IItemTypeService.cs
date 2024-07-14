namespace ProductCatalog.Application.Services.Abstraction;

public interface IItemTypeService
{
    Task<List<ProductTypeDto>> GetItemTypesAsync();

    Task<ProductTypeDto> GetItemTypeByIdAsync(Guid id);

    Task<ProductTypeDto> CreateItemTypeAsync(ProductTypeDto itemType);

    Task<ProductTypeDto> UpdateItemTypeAsync(ProductTypeDto itemType);

    Task DeleteItemTypeAsync(Guid id, bool isSoftDeleting);
}
