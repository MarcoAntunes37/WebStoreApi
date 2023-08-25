namespace WebStoreApi.Collections.ViewModels.Users.Update
{
    public class UpdateCreditCardRequest
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Cvv { get; set; }
    }
}
