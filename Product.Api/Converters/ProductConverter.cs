using Product.Api.Contracts;

namespace Product.Api.Converters
{
    public class ProductConverter : IProductConverter
    {
        public Models.Product ToModel(ProductContract contract)
        {
            return new Models.Product()
            {
                Name = contract.Name,
                Description = contract.Description,
                Price = contract.Price,
                Stock = contract.Stock,
                Category = contract.Category,
            };
        }
    }
}
