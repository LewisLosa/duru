using Microsoft.EntityFrameworkCore;

namespace DataTest;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }

    // Retrieves products whose names contain the specified substring (read-only)
    public async Task<List<Product>> GetProductsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await Context.Products
            .AsNoTracking()
            .Where(p => p.Name.Contains(name))
            .ToListAsync(cancellationToken);
    }
}