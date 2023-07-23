using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

  namespace WebStoreApi.Collections;
public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Telephone { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }    
    public string Email { get; set; }
    public string Token { get; set; } = string.Empty;    
}