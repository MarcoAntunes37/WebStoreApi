namespace WebStoreApi.Collections.ViewModels.Users.Register
{
    public class RegisterCartItemsRequest
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
