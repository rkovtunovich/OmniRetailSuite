namespace Retail.Core.Repositories;

public interface IRetailRepository<TEntity> where TEntity : EntityBase
{
    Task<IEnumerable<TEntity>> GetEntitiesAsync();

    Task<TEntity?> GetEntityAsync(Guid id);

    Task<TEntity> AddEntityAsync(TEntity entity);

    Task<TEntity> UpdateEntityAsync(TEntity entity);

    Task DeleteEntityAsync(Guid id, bool isSoftDeleting);
}
