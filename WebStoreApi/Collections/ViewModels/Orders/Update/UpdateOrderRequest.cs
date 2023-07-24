using MongoDB.Bson;

namespace WebStoreApi.Collections.ViewModels.Orders.Update
{
    public class UpdateOrderRequest
    {
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderStatus { get; set; }
    }
}
