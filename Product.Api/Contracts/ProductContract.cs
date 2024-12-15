namespace Product.Api.Contracts
{
    public class ProductContract
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public int Stock { get; set; }
    }
}
