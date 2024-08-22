namespace RetailAssistant.Application.Services.Abstraction;

public interface IRetailDataService<TModel> : IDataService<TModel> where TModel : class
{
    event Func<TModel, Task> OnChanged;

    Task<TModel?> GetByIdAsync(Guid id);

    Task<bool> UpdateAsync(TModel model);

    Task<bool> DeleteAsync(Guid id, bool isSoftDeleting);
}
