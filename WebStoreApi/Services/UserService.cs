using Microsoft.Extensions.Options;
using MongoDB.Bson;
using BCryptNet = BCrypt.Net.BCrypt;
using MongoDB.Driver;
using WebStoreApi.Collections;
using WebStoreApi.Helpers;
using WebStoreApi.Collections.ViewModels.Users.Authorization;
using WebStoreApi.Authorization;
using AutoMapper;
using System.Security.Claims;
using WebStoreApi.Collections.ViewModels.Users.Register;
using WebStoreApi.Collections.ViewModels.Users.Update;
using System;

namespace WebStoreApi.Services
{
    public interface IUsersService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        Task<List<User>> GetAsync();
        Task<User> GetAsync(ObjectId id);
        Task CreateAsync(RegisterProfileRequest model);
        Task UpdateProfileAsync(ObjectId id, UpdateProfileRequest model);
        Task RemoveAsync(ObjectId id);        
        Task <User>GetByUsername(string username);
    }

    public class UsersService : IUsersService
    {
        private readonly IMongoCollection<User> _usersCollection;
        private IJwtUtils _jwtUtils;
        private readonly IMapper _mapper;

        public UsersService(IOptions<WebStoreDatabaseSettings> webStoreDatabaseSettings,
            IJwtUtils jwtUtils,
            IMapper mapper)
        {
            var mongoClient = new MongoClient(webStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(webStoreDatabaseSettings.Value.DatabaseName);

            _usersCollection = mongoDatabase.GetCollection<User>(webStoreDatabaseSettings.Value.UsersCollectionName);

            _jwtUtils = jwtUtils;

            _mapper = mapper;
        }

        #region Authentication
        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var user = await _usersCollection.Find(x => x.Username == model.Username).FirstOrDefaultAsync();

            if (user == null || !BCryptNet.Verify(model.Password, user.Password))
                throw new Exception("Username or password is incorrect");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            var accessToken = _jwtUtils.GenerateAccessToken(claims);
            

            user.RefreshToken = accessToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            var response = _mapper.Map<AuthenticateResponse>(user);

            return response;
        }

        public async Task UpdateRefreshToken(string username, string refreshToken, DateTime refreshTokenExpire)
        {
            var existingUser = await _usersCollection.Find(x => x.Username == username).FirstOrDefaultAsync();

            if (existingUser == null)
                throw new Exception("User not found");

            existingUser.RefreshToken = refreshToken;

            existingUser.RefreshTokenExpiryTime = refreshTokenExpire;

            await _usersCollection.ReplaceOneAsync(x => x.Username == username, existingUser);
        }
        #endregion

        #region User
        public async Task<List<User>> GetAsync()
        {
            var users = await _usersCollection.Find(x => true).ToListAsync();

            if (users == null)
                throw new Exception("Users not found");

            return users;
        }

        public async Task<User> GetAsync(ObjectId id)
        {
            var existingUser = await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (existingUser == null)
                throw new Exception("User not found");

            return existingUser;
        }

        public async Task RemoveAsync(ObjectId id)
        {
            var existingUser = _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (existingUser == null)
                throw new Exception("User not found");

            await _usersCollection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<User> GetByUsername(string username)
        {
            var user = await _usersCollection.Find(x => x.Username == username).FirstOrDefaultAsync();

            if (user == null)
                throw new Exception("Username not found");

            return user;
        }
        #endregion

        #region User Profile
        public async Task CreateAsync(RegisterProfileRequest model)
        {
            var existingUser = await _usersCollection.Find(x => x.Email == model.Email 
            || x.Username == model.Username).FirstOrDefaultAsync();            

            if (existingUser != null)            
                throw new Exception("Email or username is already in use");

            if (model.Password != model.ConfirmPassword)
                throw new Exception("Password and confirm password must be the same");

            var user = _mapper.Map<User>(model);

            model.Password = BCryptNet.HashPassword(model.Password, BCryptNet.GenerateSalt(12));

            await _usersCollection.InsertOneAsync(user);
        }

        public async Task UpdateProfileAsync(ObjectId id, UpdateProfileRequest model)
        {
            var existingUser = await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (existingUser == null)
                throw new Exception("User not found.");
            
            var duplicateUser = await _usersCollection.Find(x => (x.Email == model.Email) && x.Id != id).FirstOrDefaultAsync();

            if (duplicateUser != null)
                throw new Exception("Email is already in use by another user.");            

            if (model.Password != model.ConfirmPassword)
                throw new Exception("Password and confirm password must be the same");

            if (!string.IsNullOrEmpty(model.Password))
                existingUser.Password = BCryptNet.HashPassword(model.Password, BCryptNet.GenerateSalt(12));

            _mapper.Map(model, existingUser);

            await _usersCollection.ReplaceOneAsync(x => x.Id == id, existingUser);
        }
        #endregion

        #region User Adress
        private async Task UpdateAdressAsync(ObjectId id, UpdateAddressRequest model)
        {
            var existingUser = await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (existingUser == null) 
                throw new Exception("User not found");

            var filter = Builders<User>.Filter.Eq("_id", id);

            Address newAddress = new Address
            {
                Street = model.Street,
                City = model.City,
                State = model.State,
                PostalCode = model.PostalCode
            };

            var update = Builders<User>.Update.Set(u => u.BillingAddress, newAddress)
                                          .Set(u => u.ShippingAddress, newAddress);


            await _usersCollection.UpdateOneAsync(filter, update) ;
        }

        #endregion

        #region User Payment Data
        private async Task UpdateCCInfoAsync(ObjectId id, UpdateCCInfoRequest model)
        {
            var existingUser = await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (existingUser == null)
                throw new Exception("User not found");

            var filter = Builders<User>.Filter.Eq("_id", id);

            PaymentInformation newPayment = new PaymentInformation
            {
                CreditCardNumber = model.CreditCardNumber,
                ExpirationDate = model.ExpirationDate,
                CVV = model.CVV,
            };

            var update = Builders<User>.Update.Set(u => u.PaymentInfo, newPayment);


            await _usersCollection.UpdateOneAsync(filter, update);
        }
        #endregion
    }
}
