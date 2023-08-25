using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WebStoreApi.Collections
{
    public class Address
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id {  get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
    }
}
