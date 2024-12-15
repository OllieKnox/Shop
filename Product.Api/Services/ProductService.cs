
using Exceptions;
using Microsoft.EntityFrameworkCore;
using Product.Api.Contracts;
using Product.Api.Converters;
using Product.Api.Repositories;

namespace Product.Api.Services
{
    public class ProductService(IProductConverter productConverter, IProductRepository productRepository) : IProductService
    {
        public async Task<List<Models.Product>> GetProducts()
        {
            return await productRepository.GetProducts();
        }

        public async Task<Models.Product> GetProduct(int id)
        {
            var product = await productRepository.GetProduct(id);

            if (product == null)
            {
                throw new EntityNotFoundException($"Product with id {id} not found");
            }

            return product;
        }

        public async Task<Models.Product> CreateProduct(ProductContract productContract)
        {
            var product = productConverter.ToModel(productContract);
            await productRepository.CreateProduct(product);
            return product;
        }

        public async Task UpdateProduct(int id, ProductContract productContract)
        {
            var product = productConverter.ToModel(productContract);
            product.Id = id;

            try
            {
                await productRepository.UpdateProduct(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!productRepository.ProductExists(id))
                {
                    throw new EntityNotFoundException($"Product with id {id} not found");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task DeleteProduct(int id)
        {
            await productRepository.DeleteProduct(id);
        }
    }
}
