using AutoMapper;
using BackOffice.Application.Helpers;
using BackOffice.Application.Services.Abstraction.Retail;

namespace BackOffice.Application.Services.Implementation.Retail;

public class RetailService<TModel, TDto>: IRetailService<TModel> where TModel : EntityModelBase, new()
{
    private readonly IHttpService<RetailResource> _httpService;
    private readonly ILogger<RetailService<TModel, TDto>> _logger;
    private readonly IMapper _mapper;

    public event Func<TModel, Task>? OnChanged;

    public RetailService(IHttpService<RetailResource> httpService, ILogger<RetailService<TModel, TDto>> logger, IMapper mapper)
    {
        _httpService = httpService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<List<TModel>> GetAllAsync()
    {
        var uri = RetailUrlHelper.GetAll<TModel>();
        var dtos = await _httpService.GetAsync<List<TDto>>(uri);

        var all = dtos?.Select(x => _mapper.Map<TModel>(x)).ToList();

        return all ?? [];
    }

    public async Task<TModel?> GetByIdAsync(Guid id)
    {
        var uri = RetailUrlHelper.Get<TModel>(id);
        var dto = await _httpService.GetAsync<TDto>(uri);

        return _mapper.Map<TModel>(dto);
    }

    public async Task<TModel> CreateAsync(TModel model)
    {
        var uri = RetailUrlHelper.Add<TModel>();
        await _httpService.PostAsync(uri, _mapper.Map<TDto>(model));

        OnChanged?.Invoke(model);

        return model;
    }

    public async Task<bool> UpdateAsync(TModel model)
    {
        var uri = RetailUrlHelper.Update<TModel>();
        await _httpService.PutAsync(uri, _mapper.Map<TDto>(model));

        OnChanged?.Invoke(model);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, bool isSoftDeleting)
    {
        var uri = RetailUrlHelper.Delete<TModel>(id, isSoftDeleting);
        await _httpService.DeleteAsync(uri);

        OnChanged?.Invoke(new TModel { Id = id });

        return true;
    }
}
