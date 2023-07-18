using System;
using System.Threading.Tasks;
using WebStoreApi.Services;
using WebStoreApiUTest.Mockdata;
using WebStoreApi.Collections;
using FluentAssertions;
using Xunit;
using WebStoreApi.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace WebStoreApiUTest.System.Services
{
    public class TestUserService : IDisposable
    {
        private readonly IMongoCollection<User> _usersCollection;

        public TestUserService(IOptions<WebStoreDatabaseSettings> webStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(webStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(webStoreDatabaseSettings.Value.DatabaseName);

            _usersCollection = mongoDatabase.GetCollection<User>(webStoreDatabaseSettings.Value.UsersCollectionName);
        }
    }
}
