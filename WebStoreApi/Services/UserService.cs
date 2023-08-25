using Microsoft.Extensions.Options;
using BCryptNet = BCrypt.Net.BCrypt;
using MongoDB.Driver;
using WebStoreApi.Collections;
using WebStoreApi.Helpers;
using WebStoreApi.Collections.ViewModels.Users.Authorization;
using AutoMapper;
using WebStoreApi.Collections.ViewModels.Users.Register;
using WebStoreApi.Collections.ViewModels.Users.Update;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using WebStoreApi.Interfaces;
using MongoDB.Bson;

namespace WebStoreApi.Services
{
    public class UsersService : IUsersService
    {
        private readonly IMongoCollection<User> _usersCollection;

        private readonly IMapper _mapper;

        private readonly IConfiguration _configuration;


        public UsersService(IOptions<WebStoreDatabaseSettings> webStoreDatabaseSettings,
            IMapper mapper,
            IConfiguration configuration)
        {
            var mongoClient = new MongoClient(webStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(webStoreDatabaseSettings.Value.DatabaseName);

            _usersCollection = mongoDatabase.GetCollection<User>(webStoreDatabaseSettings
                .Value.UsersCollectionName);

            _mapper = mapper;

            _configuration = configuration;
        }

        #region Authentication
        public async Task<AuthenticateResponse> Login(AuthenticateRequest model)
        {
            var user = await _usersCollection.Find(x => x.Username == model.Username)
                .FirstOrDefaultAsync();

            if (user == null)
                throw new Exception("User not found");

            if (!BCryptNet.Verify(model.Password, user.Password))
                throw new Exception(message: "Username or password is incorrect");

            var tokenHandler = new JwtSecurityTokenHandler();

            var secret = Encoding.UTF8.GetBytes(_configuration["JsonWebToken:Secret"]!);

            var tokenDescr = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescr);

            var tokenR = tokenHandler.WriteToken(token);

            var response = _mapper.Map<AuthenticateResponse>(user);

            response.Token = tokenR;

            return response;
        }
        #endregion
        #region User
        public async Task<List<User>> GetAsync()
        {
            var users = await _usersCollection.Find(x => true).ToListAsync();

            if (users == null)
                throw new Exception(message: "Users not found");

            return users;
        }

        public async Task<User> GetAsync(string id)
        {
            var existingUser = await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (existingUser == null)
                throw new Exception(message: "User not found");

            return existingUser;
        }

        public async Task RemoveAsync(string id)
        {
            var existingUser = await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (existingUser == null)
                throw new Exception(message: "User not found");

            await _usersCollection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            var user = await _usersCollection.Find(x => x.Username == username).FirstOrDefaultAsync();

            if (user == null)
                throw new Exception(message: "Username not found");

            return user;
        }
        #endregion

        #region User Profile
        public async Task CreateAsync(RegisterProfileRequest model)
        {
            var existingUser = await _usersCollection.Find(x => x.Email == model.Email || x.Username == model.Username)
                .FirstOrDefaultAsync();

            if (existingUser != null)
                throw new Exception(message: "Username or email is already in use");

            if (model.Password != model.ConfirmPassword)
                throw new Exception(message: "Password and confirm password must be the same");


            model.Password = BCryptNet.HashPassword(model.Password, BCryptNet.GenerateSalt(12));

            var user = _mapper.Map<User>(model);

            await _usersCollection.InsertOneAsync(user);

        }

        public async Task UpdateProfileAsync(string id, UpdateProfileRequest model)
        {
            var user = await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (user == null)
                throw new Exception(message: "User not found.");

            var existingUser = await _usersCollection.Find(x => (x.Email == model.Email) && x.Id != id)
                .FirstOrDefaultAsync();

            if (existingUser != null)
                throw new Exception(message: "Email is already in use by another user.");

            _mapper.Map(model, user);

            await _usersCollection.ReplaceOneAsync(x => x.Id == id, user);
        }
        #endregion

        public async Task UpdatePasswordAsync(string userId, UpdatePasswordRequest model)
        {
            var user = await _usersCollection.Find(x => x.Id==userId).FirstOrDefaultAsync();

            if (user == null) throw new Exception(message: "User not found");

            if (!BCryptNet.Verify(model.Password, user.Password))
                throw new Exception(message: "User password dont match");                
            
            if (model.NewPassword != model.ConfirmPassword)
                throw new Exception(message: "Password and confirm password must be the same");

            var hashed = BCryptNet.HashPassword(model.NewPassword, BCryptNet.GenerateSalt(12));

            if (hashed != user.Password)
            {
                model.Password = hashed;
            }
            else
            {
                throw new Exception(message: "New password cant be equal to password");
            }

            _mapper.Map(model, user);

            await _usersCollection.ReplaceOneAsync(x => x.Id == userId, user);
        }
        public async Task InsertAddress(string userId, RegisterAddressRequest model)
        {
            var user = await _usersCollection.Find(x => x.Id == userId).FirstOrDefaultAsync();

            if (user == null) throw new Exception(message: "User not found");

            var address = _mapper.Map<Address>(model);

            address.Id = new BsonObjectId(ObjectId.GenerateNewId()).ToString();

            user.Addresses.Add(address);

            await _usersCollection.ReplaceOneAsync(x => x.Id == userId, user);
        }

        public async Task UpdateAddress(string userId, string addressId, UpdateAddressRequest model)
        {
            var user = await _usersCollection.Find(x => x.Id == userId).FirstOrDefaultAsync();

            if (user == null) throw new Exception("User not found");

            var index = user.Addresses.FindIndex(a => a.Id == addressId);

            var address = user.Addresses.FirstOrDefault(address => address.Id == addressId);

            _mapper.Map(model, address);

            user.Addresses.RemoveAt(index);

            user.Addresses.Insert(index, address!);

            await _usersCollection.ReplaceOneAsync(x => x.Id == userId, user);
        }

        public async Task DeleteAddress(string userId, string addressId)
        {
            var user = await _usersCollection.Find(x => x.Id == userId).FirstOrDefaultAsync();

            if (user == null) throw new Exception("User not found");

            var address = user.Addresses.FirstOrDefault(x => x.Id == addressId);

            user.Addresses.Remove(address!);

            await _usersCollection.ReplaceOneAsync(x => x.Id == userId, user);
        }

        public async Task InsertCreditCard(string userId, RegisterCreditCardRequest model)
        {
            var user = await _usersCollection.Find(x => x.Id == userId).FirstOrDefaultAsync();

            if (user == null) throw new Exception("User not found");

            var creditCard = _mapper.Map<CreditCard>(model);

            creditCard.Id = new BsonObjectId(ObjectId.GenerateNewId()).ToString();

            user.CreditCards.Add(creditCard);

            await _usersCollection.ReplaceOneAsync(x => x.Id == userId, user);
        }

        public async Task UpdateCreditCard(string userId, string creditCardId, UpdateCreditCardRequest model)
        {
            var user = await _usersCollection.Find(x => x.Id == userId).FirstOrDefaultAsync();

            if (user == null) throw new Exception("User not found");

            var index = user.CreditCards.FindIndex(a => a.Id == creditCardId);

            var creditCard = user.CreditCards.FirstOrDefault(address => address.Id == creditCardId);

            _mapper.Map(model, creditCard);

            user.CreditCards.RemoveAt(index);

            user.CreditCards.Insert(index, creditCard!);

            await _usersCollection.ReplaceOneAsync(x => x.Id == userId, user);
        }

        public async Task DeleteCreditCard(string userId, string creditCardId)
        {
            var user = await _usersCollection.Find(x => x.Id == userId).FirstOrDefaultAsync();

            if (user == null) throw new Exception("User not found");

            var creditCard = user.CreditCards.FirstOrDefault(x => x.Id == creditCardId);

            user.CreditCards.Remove(creditCard!);

            await _usersCollection.ReplaceOneAsync(x => x.Id == userId, user);
        }

    }
}