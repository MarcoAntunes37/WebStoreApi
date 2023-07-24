﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WebStoreApi.Collections.ViewModels.Products.Register
{
    public class RegisterProductRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }        
    }
}
