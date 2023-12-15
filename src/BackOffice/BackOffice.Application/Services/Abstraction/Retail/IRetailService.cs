namespace BackOffice.Application.Services.Abstraction.Retail;

public interface IRetailService<TModel> where TModel : class
{
    event Func<TModel, Task> OnChanged;

    Task<TModel?> GetByIdAsync(Guid id);

    Task<List<TModel>> GetAllAsync();

    Task<TModel> CreateAsync(TModel cashierDto);

    Task<bool> UpdateAsync(TModel cashierDto);

    Task<bool> DeleteAsync(Guid id, bool isSoftDeleting);
}
