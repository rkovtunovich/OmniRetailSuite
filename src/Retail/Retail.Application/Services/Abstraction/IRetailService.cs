namespace Retail.Application.Services.Abstraction;

public interface IRetailService<TDto>
{
    Task<TDto?> GetByIdAsync(Guid id);

    Task<List<TDto>> GetAllAsync();

    Task<TDto> CreateAsync(TDto entity);

    Task<bool> UpdateAsync(TDto entity);

    Task<bool> DeleteAsync(Guid id, bool isSoftDeleting);
}
