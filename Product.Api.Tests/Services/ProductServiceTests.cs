using Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Product.Api.Contracts;
using Product.Api.Converters;
using Product.Api.Repositories;
using Product.Api.Services;

namespace Product.Api.Tests.Services
{
    [TestClass]
    public class ProductServiceTests
    {
        private Mock<IProductRepository>? mockProductRepository;
        private ProductService? productService;

        [TestInitialize]
        public void Initialize()
        {
            mockProductRepository = new Mock<IProductRepository>();
            productService = new ProductService(new ProductConverter(), mockProductRepository.Object);
        }

        [TestMethod]
        public async Task GetProducts_ReturnsList()
        {
            List<Models.Product> products = [new() { Id = 1, Name = "Product 1", Description = "Description 1", Price = 10.00, Stock = 250 }];
            mockProductRepository!.Setup(service => service.GetProducts()).ReturnsAsync(products);

            var result = await productService!.GetProducts();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<Models.Product>));
            Assert.AreEqual(products.Count, result.Count);
        }

        [TestMethod]
        public async Task GetProduct_ReturnsObject()
        {
            var product = new Models.Product { Id = 1, Name = "Product 1", Description = "Description 1", Price = 10.00, Stock = 250 };
            mockProductRepository!.Setup(service => service.GetProduct(It.IsAny<int>())).ReturnsAsync(product);

            var result = await productService!.GetProduct(product.Id);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Models.Product));
        }

        [TestMethod]
        public async Task GetProduct_ThrowsEntityNotFoundException()
        {
            var action = () => productService!.GetProduct(1);
            await Assert.ThrowsExceptionAsync<EntityNotFoundException>(action);
        }

        [TestMethod]
        public async Task CreateProduct_ReturnsObject()
        {
            var productContract = new ProductContract { Name = "Product 1", Description = "Description 1", Price = 10.00, Stock = 250 };
            var result = await productService!.CreateProduct(productContract);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Models.Product));
        }

        [TestMethod]
        public async Task UpdateProduct_Returns()
        {
            var productContract = new ProductContract { Name = "Product 1", Description = "Description 1", Price = 10.00, Stock = 250 };
            await productService!.UpdateProduct(1, productContract);
        }

        [TestMethod]
        public async Task UpdateProduct_ThrowsEntityNotFoundException()
        {
            var productContract = new ProductContract { Name = "Product 1", Description = "Description 1", Price = 10.00, Stock = 250 };
            mockProductRepository!.Setup(service => service.UpdateProduct(It.IsAny<Models.Product>())).ThrowsAsync(new DbUpdateConcurrencyException());
            mockProductRepository!.Setup(service => service.ProductExists(It.IsAny<int>())).Returns(false);

            var action = () => productService!.UpdateProduct(1, productContract);

            await Assert.ThrowsExceptionAsync<EntityNotFoundException>(action);
        }

        [TestMethod]
        public async Task UpdateProduct_ThrowsDbUpdateConcurrencyException()
        {
            var productContract = new ProductContract { Name = "Product 1", Description = "Description 1", Price = 10.00, Stock = 250 };
            mockProductRepository!.Setup(service => service.UpdateProduct(It.IsAny<Models.Product>())).ThrowsAsync(new DbUpdateConcurrencyException());
            mockProductRepository!.Setup(service => service.ProductExists(It.IsAny<int>())).Returns(true);

            var action = () => productService!.UpdateProduct(1, productContract);

            await Assert.ThrowsExceptionAsync<DbUpdateConcurrencyException>(action);
        }

        [TestMethod]
        public async Task DeleteProduct_Returns()
        {
            await productService!.DeleteProduct(1);
        }
    }
}
