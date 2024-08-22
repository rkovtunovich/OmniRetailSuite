namespace RetailAssistant.Application.Services.Abstraction;

public interface IProductCatalogDataService<TModel> : IDataService<TModel> where TModel : class
{
    event Func<TModel, Task> OnChanged;

    Task<TModel?> GetByIdAsync(Guid id);

    Task<IList<TModel>> GetByParentAsync(Guid parentId);

    Task<bool> UpdateAsync(TModel model);

    Task<bool> DeleteAsync(Guid id, bool isSoftDeleting);
}
