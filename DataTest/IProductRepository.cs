namespace DataTest;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetProductsByNameAsync(string name, CancellationToken cancellationToken = default);
}