using System.ComponentModel.DataAnnotations;

namespace WebStoreApi.Collections.ViewModels.Users.Update
{
    public class UpdateProfileRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Telephone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        
    }
}
