using System.Linq.Expressions;

namespace XoomCore.Infrastructure.Repositories.Contracts;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetAsync(long id);
    IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> expression);
    IQueryable<TEntity> GetAll();
    Task InsertAsync(TEntity entity);
    Task InsertRangeAsync(IEnumerable<TEntity> entities);
    Task UpdateAsync(TEntity oldEntity, TEntity newEntity);
    Task UpdateAsync(TEntity entity);
    Task UpdateRangeAsync(IEnumerable<TEntity> entities);
    Task DeleteAsync(TEntity entity);
    Task DeleteRangeAsync(IEnumerable<TEntity> entities);
}
