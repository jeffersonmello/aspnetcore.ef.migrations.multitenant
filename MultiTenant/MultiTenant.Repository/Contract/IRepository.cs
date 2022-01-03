using System.Linq.Expressions;

namespace MultiTenant.Repository.Contract;

public interface IRepository<TEntity, TKey>
{
    Task<List<TEntity>> Select();

    Task<List<TEntity>> Select(Expression<Func<TEntity, bool>> predicate);

    Task<TEntity?> SelectById(TKey id);

    Task<TEntity> Save(TEntity entity);

    Task<TEntity> Update(TEntity entity);

    Task Delete(TEntity entity);

    Task DeleteById(TKey id);
}