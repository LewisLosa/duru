using DataTest;
using Microsoft.EntityFrameworkCore;

namespace TestProject1;

public class Tests
{
    [TestFixture]
    public class ProductRepositoryWorkflowTest
    {
        private AppDbContext _context;
        private ProductRepository _repository;
        
        [OneTimeSetUp]
        public void Setup()
        {
            // Create a single in-memory database for the entire test workflow
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ProductWorkflowTestDB")
                .Options;
                
            _context = new AppDbContext(options);
            _repository = new ProductRepository(_context);
            
            TestContext.Out.WriteLine("Test database and repository created");
        }
        
        [Test]
        public async Task ProductRepository_CompleteWorkflow()
        {
            // STEP 1: Add a new product
            TestContext.Out.WriteLine("STEP 1: Adding a new product...");
            var product = new Product { Name = "Test Product", Price = 9.99m };
            await _repository.AddAsync(product);
            await _repository.SaveChangesAsync();
            
            TestContext.Out.WriteLine($"Product added with ID: {product.Id}, Name: {product.Name}, Price: {product.Price}");
            
            // Verify product was added
            var addedProduct = await _repository.GetByIdAsync(product.Id);
            Assert.That(addedProduct, Is.Not.Null, "Product should be added to database");
            Assert.That(addedProduct.Name, Is.EqualTo("Test Product"), "Product name should match");
            Assert.That(addedProduct.Price, Is.EqualTo(9.99m), "Product price should match");
            TestContext.Out.WriteLine("Add product verification successful");
            
            // STEP 2: List/Retrieve products
            TestContext.Out.WriteLine("\nSTEP 2: Listing all products...");
            var allProducts = _repository.GetAllAsync();
            TestContext.Out.WriteLine($"Total products found: {allProducts.Count()}" as object);
            
            foreach (var p in allProducts)
            {
                TestContext.Out.WriteLine($"Found product - ID: {p.Id}, Name: {p.Name}, Price: {p.Price}");
            }
            
            Assert.That(allProducts.Count, Is.GreaterThan(0), "Product list should not be empty");
            TestContext.Out.WriteLine("List products verification successful");
            
            
            // STEP 3: Update product
            TestContext.Out.WriteLine("\nSTEP 3: Updating product...");
            product.Name = "Updated Product Name";
            product.Price = 19.99m;
            
            _repository.Update(product);
            await _repository.SaveChangesAsync();
            TestContext.Out.WriteLine($"Product updated - ID: {product.Id}, New Name: {product.Name}, New Price: {product.Price}");
            
            // Verify update
            var updatedProduct = await _repository.GetByIdAsync(product.Id);
            Assert.That(updatedProduct.Name, Is.EqualTo("Updated Product Name"), "Product name should be updated");
            Assert.That(updatedProduct.Price, Is.EqualTo(19.99m), "Product price should be updated");
            TestContext.Out.WriteLine("Update product verification successful");
            
            // STEP 4: Delete product
            TestContext.Out.WriteLine("\nSTEP 4: Deleting product...");
            _repository.Delete(product);
            await _repository.SaveChangesAsync();
            TestContext.Out.WriteLine($"Product deleted - ID: {product.Id}");
            
            // Verify deletion
            var deletedProduct = await _repository.GetByIdAsync(product.Id);
            Assert.That(deletedProduct, Is.Null, "Product should be removed from database");
            TestContext.Out.WriteLine("Delete product verification successful");
            
            // Verify database is empty
            var remainingProducts = _repository.GetAllAsync();
            Assert.That(remainingProducts.Count, Is.EqualTo(0), "Database should be empty after deletion");
            TestContext.Out.WriteLine("\nComplete workflow test passed successfully!");
        }
        
        [OneTimeTearDown]
        public void Cleanup()
        {
            // Clean up resources
            _context.Dispose();
            TestContext.Out.WriteLine("Test completed and resources cleaned up");
        }
    }
}