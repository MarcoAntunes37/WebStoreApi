using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using WebStoreApi.Collections;
using System.Security.Claims;
using WebStoreApi.Helpers;
using AutoMapper;
using System.Runtime.InteropServices;
using WebStoreApi.Collections.ViewModels.Products.Register;
using WebStoreApi.Collections.ViewModels.Products.Update;

namespace WebStoreApi.Services
{
    public interface IProductsService
    {
        Task<List<Product>> GetAsync();
        Task<Product> GetAsync(ObjectId id);
        Task CreateAsync(RegisterProductRequest model);
        Task UpdateAsync(ObjectId id, UpdateProductRequest model);
        Task RemoveAsync(ObjectId id);
    }

    public class ProductsService : IProductsService
    {
        private readonly IMongoCollection<Product> _productsColection;
        private readonly IMapper _mapper;
        

        public ProductsService(IOptions<WebStoreDatabaseSettings> webStoreDatabaseSettings,
            IMapper mapper)
        {
            var mongoClient = new MongoClient(webStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(webStoreDatabaseSettings.Value.DatabaseName);

            _productsColection = mongoDatabase.GetCollection<Product>(webStoreDatabaseSettings.Value.ProductsCollectionName);

            _mapper = mapper;
        }

        public async Task<List<Product>> GetAsync()
        {
            var product = await _productsColection.Find(_ => true).ToListAsync();

            if (product == null)
                throw new Exception("No products found");

            return product;
        }

        public async Task<Product> GetAsync(ObjectId id)
        {
            var product = await _productsColection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (product == null)
                throw new Exception("Product not found");

            return product;
        }

        public async Task CreateAsync(RegisterProductRequest model)
        {
            var existingProduct = await _productsColection.Find(x => x.Name == model.Name).FirstOrDefaultAsync();

            if (existingProduct != null)
                throw new Exception("Product name already taken");

            var product = _mapper.Map<Product>(model);

            await _productsColection.InsertOneAsync(product);
        }

        public async Task UpdateAsync(ObjectId id, UpdateProductRequest model)
        {
            var existingProduct = await _productsColection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (existingProduct != null)
                throw new Exception("Product not found");

            var duplicatedProduct = await _productsColection.Find(x => (x.Name == model.Name) && x.Id != id).FirstOrDefaultAsync();

            if (duplicatedProduct != null)
                throw new Exception("Product name already taken");

            _mapper.Map(model, existingProduct);

            await _productsColection.ReplaceOneAsync(x => x.Id == id, existingProduct);
        }

        public async Task RemoveAsync(ObjectId id)
        {
            var product = _productsColection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (product == null)
                throw new Exception("Product not found");

            await _productsColection.DeleteOneAsync(x => x.Id == id);
        }
    }
}
