namespace WebStoreApi.Collections.ViewModels.Products.Update
{
    public class UpdateProductRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
    }
}
