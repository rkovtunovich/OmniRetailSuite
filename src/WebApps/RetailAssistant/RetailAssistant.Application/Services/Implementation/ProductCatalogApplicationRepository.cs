using Microsoft.JSInterop;

namespace RetailAssistant.Application.Services.Implementation;

public class ProductCatalogApplicationRepository<TModel> : IApplicationRepository<TModel> where TModel : class
{
    private const string _productCatalogDbName = "productCatalog";

    private readonly IJSRuntime _jsRuntime;
    private readonly IApplicationStateService _applicationStateService;

    public ProductCatalogApplicationRepository(IJSRuntime jsRuntime, IApplicationStateService applicationStateService)
    {
        _jsRuntime = jsRuntime;
        _applicationStateService = applicationStateService;
    }

    public Task<TModel> CreateAsync(TModel model)
    {
        throw new NotImplementedException();
    }

    public Task<IList<TModel>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<TModel?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(TModel model)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Guid id, bool isSoftDeleting)
    {
        throw new NotImplementedException();
    }
}
