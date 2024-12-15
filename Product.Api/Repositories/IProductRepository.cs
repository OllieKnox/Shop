namespace Product.Api.Repositories
{
    public interface IProductRepository
    {
        Task<List<Models.Product>> GetProducts();
        Task<Models.Product?> GetProduct(int id);
        Task CreateProduct(Models.Product product);
        Task UpdateProduct(Models.Product product);
        Task DeleteProduct(int id);
        bool ProductExists(int id);
    }
}
