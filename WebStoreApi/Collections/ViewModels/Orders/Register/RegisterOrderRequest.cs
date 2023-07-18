using MongoDB.Bson;

namespace WebStoreApi.Collections.ViewModels.Orders.Register
{
    public class RegisterOrderRequest
    {
        public ObjectId UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime PreferredDeliveryDate { get; set; }
        public string DeliveryInstructions { get; set; }
        public string OrderStatus { get; set; }
        public DateTime RentContractStartDate { get; set; }
        public DateTime RentContractEndDate { get; set; }
        public OrderPaymentInformation PaymentInfo { get; set; }
    }
}
