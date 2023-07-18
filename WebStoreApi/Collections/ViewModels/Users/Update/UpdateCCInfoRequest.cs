namespace WebStoreApi.Collections.ViewModels.Users.Update
{
    public class UpdateCCInfoRequest
    {
        public string CreditCardNumber { get; set; }
        public string ExpirationDate { get; set; }
        public string CVV { get; set; }
    }
}
