namespace WebStoreApi.Collections.ViewModels.Orders.Register
{
    public class RegisterOrderRequest
    {
        public string UserId { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public decimal Fee { get; set; }    
        public decimal SubTotal { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
