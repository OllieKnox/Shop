
using Exceptions;
using Microsoft.EntityFrameworkCore;
using Product.Api.Contexts;

namespace Product.Api.Repositories
{
    public class ProductRepository(ProductContext productContext) : IProductRepository
    {
        public async Task<List<Models.Product>> GetProducts()
        {
            return await productContext.Products.ToListAsync();
        }
        
        public async Task<Models.Product?> GetProduct(int id)
        {
            return await productContext.Products.FindAsync(id);
        }

        public async Task CreateProduct(Models.Product product)
        {
            productContext.Products.Add(product);
            await productContext.SaveChangesAsync();
        }

        public async Task UpdateProduct(Models.Product product)
        {
            productContext.Entry(product).State = EntityState.Modified;
            await productContext.SaveChangesAsync();
        }

        public async Task DeleteProduct(int id)
        {
            var product = await productContext.Products.FindAsync(id);

            if (product == null)
            {
                throw new EntityNotFoundException($"Product with id {id} not found");
            }

            productContext.Products.Remove(product);
            await productContext.SaveChangesAsync();
        }

        public bool ProductExists(int id)
        {
            return productContext.Products.Any(e => e.Id == id);
        }

    }
}
