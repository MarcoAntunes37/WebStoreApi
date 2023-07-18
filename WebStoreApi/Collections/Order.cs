using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebStoreApi.Collections;

public class Order
{
    public ObjectId Id { get; set; }
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

public class OrderItem
{
    public ObjectId ProductId { get; set; }
    public int Quantity { get; set; }
}

public class OrderPaymentInformation
{
    public string PaymentMethod { get; set; }
    public decimal AmountPaid { get; set; }
    public DateTime PaymentDate { get; set; }
    public bool PaymentDone { get; set; }
}
