using System.ComponentModel.DataAnnotations;

namespace WebStoreApi.Collections.ViewModels.Users.Update
{
    public class UpdatePasswordRequest
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string ConfirmPassword { get; set; } 
    }
}
