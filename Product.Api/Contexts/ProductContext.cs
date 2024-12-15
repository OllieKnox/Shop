using Microsoft.EntityFrameworkCore;

namespace Product.Api.Contexts
{
    public class ProductContext(DbContextOptions<ProductContext> options) : DbContext(options)
    {
        public DbSet<Models.Product> Products { get; set; }
    }
}
