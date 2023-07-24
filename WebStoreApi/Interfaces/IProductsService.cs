﻿using WebStoreApi.Collections;
using WebStoreApi.Collections.ViewModels.Products.Register;
using WebStoreApi.Collections.ViewModels.Products.Update;

namespace WebStoreApi.Interfaces
{
    public interface IProductsService
    {
        Task<List<Product>> GetAsync();
        Task<Product> GetAsync(string id);
        Task CreateAsync(RegisterProductRequest model);
        Task UpdateAsync(string id, UpdateProductRequest model);
        Task RemoveAsync(string id);
    }
}
