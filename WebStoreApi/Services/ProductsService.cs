using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebStoreApi.Collections;
using WebStoreApi.Helpers;
using AutoMapper;
using WebStoreApi.Collections.ViewModels.Products.Register;
using WebStoreApi.Collections.ViewModels.Products.Update;
using WebStoreApi.Interfaces;

namespace WebStoreApi.Services 
{ 
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
            var products = await _productsColection.Find(_ => true).ToListAsync();

            if (products == null)
                throw new Exception("No products found");

            return products;
        }

        public async Task<Product> GetAsync(string id)
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

        public async Task UpdateAsync(string id, UpdateProductRequest model)
        {
            var product = await _productsColection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (product == null)
                throw new Exception("Product not found");

            var existingProduct = await _productsColection.Find(x => (x.Name == model.Name) && x.Id != id).FirstOrDefaultAsync();

            if (existingProduct != null)
                throw new Exception("Product name already taken");

            _mapper.Map(model, product);

            await _productsColection.ReplaceOneAsync(x => x.Id == id, product);
        }

        public async Task RemoveAsync(string id)
        {
            var product = _productsColection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (product == null)
                throw new Exception("Product not found");

            await _productsColection.DeleteOneAsync(x => x.Id == id);
        }
    }
}
