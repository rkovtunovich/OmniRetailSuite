using AutoMapper;
using Contracts.Dtos;
using Core.Abstraction;
using Retail.Core.Repositories;

namespace Retail.Application.Services.Implementation;

public class RetailService<TEntity, TDto> : IRetailService<TDto> where TEntity : EntityBase where TDto : EntityDtoBase
{
    private readonly ILogger<RetailService<TEntity, TDto>> _logger;
    private readonly IMapper _mapper;
    private readonly IRetailRepository<TEntity> _repository;

    public RetailService(ILogger<RetailService<TEntity, TDto>> logger, IMapper mapper, IRetailRepository<TEntity> repository)
    {
        _logger = logger;
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<List<TDto>> GetAllAsync()
    {
        try
        {
            var all = await _repository.GetEntitiesAsync();

            return _mapper.Map<List<TDto>>(all);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting entities");
            throw;
        }
    }

    public async Task<TDto?> GetByIdAsync(Guid id)
    {
        try
        {
            var entity = await _repository.GetEntityAsync(id);

            return _mapper.Map<TDto>(entity);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while getting entity: id {id}");
            throw;
        }
    }

    public async Task<TDto> CreateAsync(TDto dto)
    {
        try
        {
            await _repository.AddEntityAsync(_mapper.Map<TEntity>(dto));

            return dto;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while creating entity: id {dto.Id}");
            throw;
        }
    }

    public async Task<bool> UpdateAsync(TDto dto)
    {
        try
        {
            await _repository.UpdateEntityAsync(_mapper.Map<TEntity>(dto));

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while getting entity: id {dto.Id}");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(Guid id, bool isSoftDeleting)
    {
        try
        {
            await _repository.DeleteEntityAsync(id, isSoftDeleting);

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while deleting entity: id {id}");
            throw;
        }
    }
}
