using Microsoft.EntityFrameworkCore;

namespace DataTest;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext Context;
    private readonly DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        Context = context;
        _dbSet = Context.Set<T>();
    }

    // Retrieves all records of type T from the database (read-only)
    public IQueryable<T> GetAllAsync()
    {
        return _dbSet.AsNoTracking().AsSingleQuery();
    }

    // Finds a record by its primary key (read-only)
    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    // Adds a new entity to the context
    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    // Updates an existing entity
    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    // Removes an entity from the context
    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    // Persists changes to the database
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await Context.SaveChangesAsync(cancellationToken);
    }
}