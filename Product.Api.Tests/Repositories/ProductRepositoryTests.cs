using Exceptions;
using Microsoft.EntityFrameworkCore;
using Product.Api.Contexts;
using Product.Api.Repositories;

namespace Product.Api.Tests.Repositories
{
    [TestClass]
    public class ProductRepositoryTests
    {
        private ProductContext? productContext;
        private ProductRepository? productRepository;

        private static readonly List<Models.Product> products =
        [
            new() { Id = 1, Name = "Product 1", Description = "Description 1", Price = 10.00, Stock = 250 },
            new() { Id = 2, Name = "Product 2", Description = "Description 2", Price = 25.00, Stock = 1000 }
        ];


        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<ProductContext>()
             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
             .Options;

            productContext = new ProductContext(options);
            productRepository = new ProductRepository(productContext);

            productContext.Database.EnsureCreated();
            productContext.AddRange(products);
            productContext.SaveChanges();
        }

        [TestMethod]
        public async Task GetProducts_ReturnsList()
        {
            var result = await productRepository!.GetProducts();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<IEnumerable<Models.Product>>(result);
            Assert.AreEqual(products.Count, result.Count);
        }

        [TestMethod]
        public async Task GetProduct_ReturnsObject()
        {
            var result = await productRepository!.GetProduct(products.First().Id);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<Models.Product>(result);
        }

        [TestMethod]
        public async Task GetProduct_ReturnsNull()
        {
            var result = await productRepository!.GetProduct(100);
            
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task CreateProduct_Returns()
        {
            var product = new Models.Product { Id = 0, Name = "New Product", Description = "New Description", Price = 9.99, Stock = 500 };
            await productRepository!.CreateProduct(product);

            Assert.AreNotEqual(0, product.Id);
        }

        [TestMethod]
        public async Task UpdateProduct_Returns()
        {
            var product = products.First();
            product.Name = "Updated Product";
            await productRepository!.UpdateProduct(product);

            var result = productContext!.Products.FirstOrDefault(p => p.Name == product.Name);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task DeleteProduct_Returns()
        {
            await productRepository!.DeleteProduct(products.First().Id);

            var result = productContext!.Products.Find(products.First().Id);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task DeleteProduct_ThrowsEntityNotFoundException()
        {
            var action = () => productRepository!.DeleteProduct(100);
            await Assert.ThrowsExceptionAsync<EntityNotFoundException>(action);
        }

        [TestCleanup]
        public void Cleanup()
        {
            productContext!.Database.EnsureDeleted();
        }
    }
}
