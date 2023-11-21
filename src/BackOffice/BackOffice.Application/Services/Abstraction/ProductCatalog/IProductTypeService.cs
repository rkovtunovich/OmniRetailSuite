using BackOffice.Core.Models.ProductCatalog;

namespace BackOffice.Application.Services.Abstraction.ProductCatalog;

public interface IProductTypeService
{
    event Func<ProductType, Task> ProductTypeChanged;

    Task<List<ProductType>> GetTypesAsync();

    Task<ProductType> GetTypeByIdAsync(Guid id);

    Task<ProductType> CreateTypeAsync(ProductType catalogType);

    Task<ProductType> UpdateTypeAsync(ProductType catalogType);

    Task DeleteTypeAsync(Guid id, bool useSoftDeleting);
}
