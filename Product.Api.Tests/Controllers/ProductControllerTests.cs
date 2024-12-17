using Moq;
using Product.Api.Services;
using Product.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Product.Api.Contracts;
using Exceptions;

namespace Product.Api.Tests.Controllers
{
    [TestClass]
    public class ProductControllerTests
    {
        private Mock<IProductService>? mockProductService;
        private ProductController? productController;

        [TestInitialize]
        public void Initialize()
        {
            mockProductService = new Mock<IProductService>();
            productController = new ProductController(mockProductService.Object);
        }

        [TestMethod]
        public async Task GetProducts_ReturnsOk_WithList()
        {
            List<Models.Product> products = [new() { Id = 1, Name = "Product 1", Description = "Description 1", Price = 10.00, Stock = 250 }];
            mockProductService!.Setup(service => service.GetProducts()).ReturnsAsync(products);

            var actionResult = await productController!.GetProducts();
            
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));

            var okObjectResult = (actionResult as OkObjectResult)!;

            Assert.IsNotNull(okObjectResult.Value);
            Assert.IsInstanceOfType(okObjectResult.Value, typeof(IEnumerable<Models.Product>));
            
            var productsResult = (okObjectResult.Value as IEnumerable<Models.Product>)!;

            Assert.AreEqual(products.Count, productsResult.Count());
        }

        [TestMethod]
        public async Task GetProduct_ReturnsOk_WithObject()
        {
            var product = new Models.Product { Id = 1, Name = "Product 1", Description = "Description 1", Price = 10.00, Stock = 250 };
            mockProductService!.Setup(service => service.GetProduct(It.IsAny<int>())).ReturnsAsync(product);

            var actionResult = await productController!.GetProduct(product.Id);

            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));

            var okObjectResult = (actionResult as OkObjectResult)!;

            Assert.IsNotNull(okObjectResult.Value);
            Assert.IsInstanceOfType(okObjectResult.Value, typeof(Models.Product));
        }

        [TestMethod]
        public async Task GetProduct_ReturnsNotFound()
        {
            var productNotFoundException = new EntityNotFoundException("Product not found");
            mockProductService!.Setup(service => service.GetProduct(It.IsAny<int>())).ThrowsAsync(productNotFoundException);

            var actionResult = await productController!.GetProduct(1);

            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task CreateProduct_ReturnsCreated_WithObject()
        {
            var productContract = new ProductContract { Name = "Product 1", Description = "Description 1", Price = 10.00, Stock = 250 };
            var product = new Models.Product {
                Id = 1, 
                Name = productContract.Name, 
                Description = productContract.Description,
                Price = productContract.Price,
                Stock = productContract.Stock 
            };
            mockProductService!.Setup(service => service.CreateProduct(It.IsAny<ProductContract>())).ReturnsAsync(product);

            var actionResult = await productController!.CreateProduct(productContract);

            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(CreatedAtActionResult));

            var createdAtActionResult = (actionResult as CreatedAtActionResult)!;

            Assert.IsNotNull(createdAtActionResult.Value);
            Assert.IsInstanceOfType(createdAtActionResult.Value, typeof(Models.Product));

            Assert.IsNotNull(createdAtActionResult.RouteValues);
            Assert.AreEqual(product.Id, createdAtActionResult.RouteValues["id"]);
        }

        [TestMethod]
        public async Task UpdateProduct_ReturnsNoContent()
        {
            var productContract = new ProductContract { Name = "Product 1", Description = "Description 1", Price = 10.00, Stock = 250 };
            var actionResult = await productController!.UpdateProduct(1, productContract);

            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task UpdateProduct_ReturnsNotFound()
        {
            var productNotFoundException = new EntityNotFoundException("Product not found");
            var productContract = new ProductContract { Name = "Product 1", Description = "Description 1", Price = 10.00, Stock = 250 };
            mockProductService!.Setup(service => service.UpdateProduct(It.IsAny<int>(), It.IsAny<ProductContract>())).ThrowsAsync(productNotFoundException);

            var actionResult = await productController!.UpdateProduct(1, productContract);

            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task DeleteProduct_ReturnsNoContent()
        {
            mockProductService!.Setup(service => service.DeleteProduct(It.IsAny<int>()));

            var actionResult = await productController!.DeleteProduct(1);

            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task DeleteProduct_ReturnsNotFound()
        {
            var productNotFoundException = new EntityNotFoundException("Product not found");
            mockProductService!.Setup(service => service.DeleteProduct(It.IsAny<int>())).ThrowsAsync(productNotFoundException);

            var actionResult = await productController!.DeleteProduct(1);

            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }
    }
}
