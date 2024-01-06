namespace BackOffice.Application.Services.Abstraction.Retail;

public interface IRetailService<TModel> where TModel : class
{
    event Func<TModel, Task> OnChanged;

    Task<TModel?> GetByIdAsync(Guid id);

    Task<List<TModel>> GetAllAsync();

    Task<TModel> CreateAsync(TModel model);

    Task<bool> UpdateAsync(TModel model);

    Task<bool> DeleteAsync(Guid id, bool isSoftDeleting);
}
