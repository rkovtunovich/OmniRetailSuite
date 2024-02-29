namespace BackOffice.Application.Services.Implementation;

public class ProductCatalogService<TModel, TDto> : IProductCatalogService<TModel> where TModel : EntityModelBase, new()
{
    private readonly IHttpService<ProductCatalogClientSettings> _httpService;
    private readonly ILogger<RetailService<TModel, TDto>> _logger;
    private readonly IMapper _mapper;
    private readonly ProductCatalogUriResolver _productCatalogUriResolver;

    public event Func<TModel, Task>? OnChanged;

    public ProductCatalogService(IHttpService<ProductCatalogClientSettings> httpService, ILogger<RetailService<TModel, TDto>> logger, IMapper mapper, ProductCatalogUriResolver productCatalogUriResolver)
    {
        _httpService = httpService;
        _logger = logger;
        _mapper = mapper;
        _productCatalogUriResolver = productCatalogUriResolver;
    }

    public async Task<IList<TModel>> GetAllAsync()
    {
        try
        {
            var uri = _productCatalogUriResolver.GetAll<TDto>();
            var dtos = await _httpService.GetAsync<List<TDto>>(uri);

            var all = dtos?.Select(x => _mapper.Map<TModel>(x)).ToList();

            return all ?? [];
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error getting all {typeof(TModel).Name}");

            return [];
        }
    }

    public async Task<TModel?> GetByIdAsync(Guid id)
    {
        var uri = _productCatalogUriResolver.Get<TDto>(id);
        var dto = await _httpService.GetAsync<TDto>(uri);

        return _mapper.Map<TModel>(dto);
    }

    public async Task<IList<TModel>> GetByParentAsync(Guid parentId)
    {
        var uri = _productCatalogUriResolver.GetCatalogItemsByParent(parentId);
        var dtos = await _httpService.GetAsync<List<TDto>>(uri);

        var all = dtos?.Select(x => _mapper.Map<TModel>(x)).ToList();

        return all ?? [];
    }

    public async Task<TModel> CreateAsync(TModel model)
    {
        var uri = _productCatalogUriResolver.Add<TDto>();
        await _httpService.PostAsync(uri, _mapper.Map<TDto>(model));

        OnChanged?.Invoke(model);

        return model;
    }

    public async Task<bool> UpdateAsync(TModel model)
    {
        var uri = _productCatalogUriResolver.Update<TDto>();
        await _httpService.PutAsync(uri, _mapper.Map<TDto>(model));

        OnChanged?.Invoke(model);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, bool isSoftDeleting)
    {
        var uri = _productCatalogUriResolver.Delete<TDto>(id, isSoftDeleting);
        await _httpService.DeleteAsync(uri);

        OnChanged?.Invoke(new TModel { Id = id });

        return true;
    }
}
