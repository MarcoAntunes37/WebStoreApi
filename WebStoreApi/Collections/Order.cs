using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WebStoreApi.Collections.ViewModels.Orders;

namespace WebStoreApi.Collections;

public class Order
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string UserId { get; set; }
    public string OrderStatus { get; set; }
    public DateTime OrderDate { get; set; }
    public List<OrderItem> OrderItems { get; set; }
    public decimal TotalPrice { get; set; }    
}

