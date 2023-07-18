using MongoDB.Bson;

namespace WebStoreApi.Collections.ViewModels.Users.Authorization
{
    public class AuthenticateResponse
    {
        public ObjectId Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
    }
}
