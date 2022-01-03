using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MultiTenant.Repository.Contract;

namespace MultiTenant.Repository.Abstract;

public abstract class RepositoryBase<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : class, new()
{
    protected DbContext _context;

    public RepositoryBase(DbContext context)
    {
        _context = context;
    }
    
    public virtual Task<List<TEntity>> Select() => _context.Set<TEntity>().ToListAsync();

    public virtual Task<List<TEntity>> Select(Expression<Func<TEntity, bool>> predicate) =>
        _context.Set<TEntity>().Where(predicate).ToListAsync();

    public virtual async Task<TEntity?> SelectById(TKey id) =>  await _context.Set<TEntity>().FindAsync(id);

    public virtual async Task<TEntity> Save(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public virtual async Task<TEntity> Update(TEntity entity)
    { ;
        _context.Set<TEntity>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return entity;
    }

    public virtual async Task Delete(TEntity entity)
    {
        _context.Set<TEntity>().Attach(entity);
        _context.Entry(entity).State = EntityState.Deleted;
        await _context.SaveChangesAsync();
    }

    public virtual async Task DeleteById(TKey id)
    {
        TEntity? entity = await SelectById(id);

        if (entity == null)
            throw new Exception("Registro não encontrado");
        
        await Delete(entity);
    }
}