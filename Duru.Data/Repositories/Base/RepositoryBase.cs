using Duru.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Duru.Data.Repositories.Base;

public class RepositoryBase<T> : IRepository<T> where T : class
{
    protected readonly DuruDbContext Context;
    private readonly DbSet<T> _dbSet;

    public RepositoryBase(DuruDbContext context)
    {
        Context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id) ?? throw new InvalidOperationException();
    }

    public async Task<IEnumerable<T?>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task AddAsync(T? entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T? entity)
    {
        _dbSet.Update(entity);
    }

    public void Remove(T? entity)
    {
        _dbSet.Remove(entity);
    }
}