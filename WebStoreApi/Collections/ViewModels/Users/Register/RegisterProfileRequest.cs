using System.ComponentModel.DataAnnotations;

namespace WebStoreApi.Collections.ViewModels.Users.Register
{
    public class RegisterProfileRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Telephone { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public bool OptInForEmails { get; set; }
        public bool OptInForSMS { get; set; }
        public SecurityQuestion SecurityQuestions { get; set; }
    }

    public class SecurityQuestion
    {
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
