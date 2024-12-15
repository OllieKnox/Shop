using Product.Api.Contracts;

namespace Product.Api.Services
{
    public interface IProductService
    {
        Task<List<Models.Product>> GetProducts();
        Task<Models.Product> GetProduct(int id);
        Task<Models.Product> CreateProduct(ProductContract productContract);
        Task UpdateProduct(int id, ProductContract productContract);
        Task DeleteProduct(int id);
    }
}
