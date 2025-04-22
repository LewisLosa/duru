using System.Linq.Expressions;
using Duru.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Duru.Data.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private readonly DbSet<T> _dbSet;
    protected readonly ApplicationDbContext Context;

    public Repository(ApplicationDbContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = Context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        // Use FindAsync for primary key lookup (potentially faster)
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        // Use AsNoTracking() for read-only queries to improve performance
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        // Set creation time (Update time is handled by SaveChanges interceptor or manually)
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        // EF Core tracks changes automatically if the entity was fetched from the context.
        // If it's detached, attach and mark as modified.
        entity.UpdatedAt = DateTime.UtcNow; // Ensure update time is set
        _dbSet.Attach(entity);
        Context.Entry(entity).State = EntityState.Modified;
        // Or if you are sure it's tracked: _dbSet.Update(entity); but Attach/Entry is safer.
    }

    public void Delete(T entity)
    {
        // If entity is tracked, just remove it.
        if (Context.Entry(entity).State == EntityState.Detached) _dbSet.Attach(entity);
        _dbSet.Remove(entity);
    }
}