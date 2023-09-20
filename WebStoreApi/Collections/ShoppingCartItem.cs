using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WebStoreApi.Collections
{
    public class ShoppingCartItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
