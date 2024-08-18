namespace RetailAssistant.Data;

public interface IApplicationRepository<TModel, TDbSchema> where TModel : class where TDbSchema : class
{
    Task<TModel?> GetByIdAsync(Guid id);

    Task<IEnumerable<TModel>> GetAllAsync();

    Task<bool> CreateAsync(TModel model);

    Task<bool> UpdateAsync(TModel model);

    Task<bool> CreateOrUpdateAsync(TModel model);

    Task<bool> DeleteAsync(Guid id);
}
