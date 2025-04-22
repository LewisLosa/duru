using System.Linq.Expressions;
using Duru.Data.Models;

namespace Duru.Data.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    // Example: FindAsync(x => x.FirstName == "John")
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    // No AddRangeAsync needed for this scope yet
    void Update(T entity); // EF Core tracks changes, so often just modify and SaveChangesAsync
    void Delete(T entity);
    // No need for explicit SaveChangesAsync here, handle it centrally (Unit of Work or Service layer)
}