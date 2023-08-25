using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebStoreApi.Collections;

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string ImageUrl { get; set; }
    public int Quantity { get; set; } = 0;
}