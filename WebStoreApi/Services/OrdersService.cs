using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebStoreApi.Collections;
using WebStoreApi.Helpers;
using AutoMapper;
using WebStoreApi.Interfaces;
using WebStoreApi.Collections.ViewModels.Orders.Register;
using WebStoreApi.Collections.ViewModels.Orders.Update;
using MongoDB.Bson;

namespace WebStoreApi.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IMongoCollection<Order> _ordersColection;

        private readonly IMapper _mapper;

        public OrdersService(IOptions<WebStoreDatabaseSettings> webStoreDatabaseSettings,
            IMapper mapper)
        {
            var mongoClient = new MongoClient(webStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(webStoreDatabaseSettings.Value.DatabaseName);

            _ordersColection = mongoDatabase.GetCollection<Order>(webStoreDatabaseSettings
                .Value.OrdersCollectionName);

            _mapper = mapper;
        }

        public async Task<List<Order>> GetAsync()
        {
            var orders = await _ordersColection.Find(_ => true).ToListAsync();

            if (orders == null)
                throw new Exception("No orders found");

            return orders;
        }

        public async Task<Order> GetAsync(string id)
        {
            var order = await _ordersColection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (order == null)
                throw new Exception("Order not found");

            return order;
        }

        public async Task<List<Order>> GetAsyncByUserId(string userId)
        {
            var orders = await _ordersColection.Find(x => x.UserId == userId).ToListAsync();

            if (orders == null)
                throw new Exception("No orders found");

            return orders;
        }

        public async Task CreateAsync(RegisterOrderRequest model)
        { 
            var order = _mapper.Map<Order>(model);

            await _ordersColection.InsertOneAsync(order);
        }

        public async Task UpdateAsync(string id, UpdateOrderRequest model)
        {
            var order = await _ordersColection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (order == null)
                throw new Exception("Order not found");                       

            _mapper.Map(model, order);

            await _ordersColection.ReplaceOneAsync(x => x.Id == id, order);
        }

        public async Task RemoveAsync(string id)
        {
            var order = _ordersColection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (order == null)
                throw new Exception("Order not found");

            await _ordersColection.DeleteOneAsync(x => x.Id == id);
        }
    }
}
