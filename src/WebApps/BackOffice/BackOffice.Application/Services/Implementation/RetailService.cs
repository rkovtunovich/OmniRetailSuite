namespace BackOffice.Application.Services.Implementation;

public class RetailService<TModel, TDto> : IRetailService<TModel> where TModel : EntityModelBase, new()
{
    private readonly IHttpService<RetailClientSettings> _httpService;
    private readonly ILogger<RetailService<TModel, TDto>> _logger;
    private readonly IMapper _mapper;
    private readonly RetailUrlResolver _retailUrlResolver;

    public event Func<TModel, Task>? OnChanged;

    public RetailService(IHttpService<RetailClientSettings> httpService, ILogger<RetailService<TModel, TDto>> logger, IMapper mapper, RetailUrlResolver retailUrlResolver)
    {
        _httpService = httpService;
        _logger = logger;
        _mapper = mapper;
        _retailUrlResolver = retailUrlResolver;
    }

    public async Task<List<TModel>> GetAllAsync()
    {
        var uri = _retailUrlResolver.GetAll<TDto>();
        var dtos = await _httpService.GetAsync<List<TDto>>(uri);

        var all = dtos?.Select(x => _mapper.Map<TModel>(x)).ToList();

        return all ?? [];
    }

    public async Task<TModel?> GetByIdAsync(Guid id)
    {
        var uri = _retailUrlResolver.Get<TDto>(id);
        var dto = await _httpService.GetAsync<TDto>(uri);

        return _mapper.Map<TModel>(dto);
    }

    public async Task<TModel> CreateAsync(TModel model)
    {
        var uri = _retailUrlResolver.Add<TDto>();
        await _httpService.PostAsync(uri, _mapper.Map<TDto>(model));

        OnChanged?.Invoke(model);

        return model;
    }

    public async Task<bool> UpdateAsync(TModel model)
    {
        var uri = _retailUrlResolver.Update<TDto>();
        await _httpService.PutAsync(uri, _mapper.Map<TDto>(model));

        OnChanged?.Invoke(model);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, bool isSoftDeleting)
    {
        var uri = _retailUrlResolver.Delete<TDto>(id, isSoftDeleting);
        await _httpService.DeleteAsync(uri);

        OnChanged?.Invoke(new TModel { Id = id });

        return true;
    }
}
