using Product.Api.Contracts;

namespace Product.Api.Converters
{
    public interface IProductConverter
    {
        public Models.Product ToModel(ProductContract contract);
    }
}
