namespace DataTest;

public interface IRepository<T> where T : class
{
    IQueryable<T> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    void Update(T entity);
    void Delete(T entity);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}