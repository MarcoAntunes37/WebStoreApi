using System.ComponentModel.DataAnnotations;

namespace WebStoreApi.Collections.ViewModels.Users.Authorization
{
    public class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
