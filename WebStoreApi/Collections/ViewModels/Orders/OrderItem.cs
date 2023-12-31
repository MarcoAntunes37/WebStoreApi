﻿namespace WebStoreApi.Collections.ViewModels.Orders
{
    public class OrderItem
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
