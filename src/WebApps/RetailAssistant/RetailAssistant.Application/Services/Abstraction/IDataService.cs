namespace RetailAssistant.Application.Services.Abstraction;

public interface IDataService<TModel> where TModel : class
{
    Task<IList<TModel>> GetAllAsync();

    Task<TModel> CreateAsync(TModel model);
}
