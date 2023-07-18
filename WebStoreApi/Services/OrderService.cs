using WebStoreApi.Collections;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using WebStoreApi.Helpers;

namespace WebStoreApi.Services;

public class OrderService
{
    private readonly IMongoCollection<Order> _booksCollection;

    public OrderService(
        IOptions<WebStoreDatabaseSettings> rentOfficeDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            rentOfficeDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            rentOfficeDatabaseSettings.Value.DatabaseName);

        _booksCollection = mongoDatabase.GetCollection<Order>(
            rentOfficeDatabaseSettings.Value.UsersCollectionName);
    }

    public async Task<List<Order>> GetAsync() =>
        await _booksCollection.Find(_ => true).ToListAsync();

    public async Task<Order?> GetAsync(ObjectId id) =>
        await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Order newOrder) =>
        await _booksCollection.InsertOneAsync(newOrder);

    public async Task UpdateAsync(ObjectId id, Order updateOrder) =>
        await _booksCollection.ReplaceOneAsync(x => x.Id == id, updateOrder);

    public async Task RemoveAsync(ObjectId id) =>
        await _booksCollection.DeleteOneAsync(x => x.Id == id);
}