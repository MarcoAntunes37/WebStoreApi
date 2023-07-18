namespace WebStoreApi.Collections.ViewModels.Products.Update
{
    public class UpdateProductRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal RentalPricePerDay { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal Suitability { get; set; }
        public decimal InsuranceValue { get; set; }
        public string ImageUrl { get; set; }
        public string WebsiteUrl { get; set; }
        public int Quantity { get; set; }
    }
}
