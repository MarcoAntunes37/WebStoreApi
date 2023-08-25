using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WebStoreApi.Collections.ViewModels.Users;

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
    public List<Address> Addresses { get; set; } = new List<Address>();
    public List<CreditCard> CreditCards { get; set; } = new List<CreditCard>();
    public Role Role { get; set; } = Role.User;
}