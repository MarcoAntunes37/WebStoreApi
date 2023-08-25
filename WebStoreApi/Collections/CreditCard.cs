using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WebStoreApi.Collections
{
    public class CreditCard
    {
        //Visa: 4; Mastercard: 51, 52, 53, 54 e 55;
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Cvv { get; set; }
    }
}
