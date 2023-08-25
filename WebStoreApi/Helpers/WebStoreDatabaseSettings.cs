namespace WebStoreApi.Helpers
{
    public class WebStoreDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string CreditCardsCollectionName { get; set; } = null!;
        public string AddressesCollectionName { get; set; } = null!;
        public string ProductsCollectionName { get; set; } = null!;
        public string UsersCollectionName { get; set; } = null!;
        public string OrdersCollectionName { get; set; } = null!;
    }
}
