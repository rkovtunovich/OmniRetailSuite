namespace RetailAssistant.Application.Services.Abstraction;

public interface IApplicationRepository<TModel> where TModel : class
{
    Task<TModel?> GetByIdAsync(Guid id);

    Task<IList<TModel>> GetAllAsync();

    Task<TModel> CreateAsync(TModel model);

    Task<bool> UpdateAsync(TModel model);

    Task<bool> DeleteAsync(Guid id, bool isSoftDeleting);
}
