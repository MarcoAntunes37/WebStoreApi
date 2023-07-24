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

            if (user == null || !BCryptNet.Verify(model.Password, user.Password))
                throw new Exception("Username or password is incorrect");

            var tokenHandler = new JwtSecurityTokenHandler();

            var secret = Encoding.UTF8.GetBytes(_configuration["JsonWebToken:Secret"]);

            var tokenDescr = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescr);

            var tokenR = tokenHandler.WriteToken(token);

            var update = Builders<User>.Update.Set(x => x.Token, tokenR);

            await _usersCollection.UpdateOneAsync(x => x.Id == user.Id, update);

            var response = _mapper.Map<AuthenticateResponse>(user);

            response.Token = tokenR;

            return response;
        }

        public async Task Logout(string id)
        {
            var user = await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (user == null)
                throw new Exception("User not found");

            if (user.Token != "" && user.Token != null)
            {
                var update = Builders<User>.Update.Set(x => x.Token, "");

                await _usersCollection.UpdateOneAsync(x => x.Id == id, update);
            }
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

        public async Task<User> GetAsync(string id)
        {
            var existingUser = await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (existingUser == null)
                throw new Exception("User not found");

            return existingUser;
        }

        public async Task RemoveAsync(string id)
        {
            var existingUser = await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

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
            var existingUser = await _usersCollection.Find(x => x.Email == model.Email || x.Username == model.Username)
                .FirstOrDefaultAsync();

            if (existingUser != null)
            {
                throw new Exception("Username or email is already in use");
            }

            if (model.Password != model.ConfirmPassword)
                throw new Exception("Password and confirm password must be the same");


            model.Password = BCryptNet.HashPassword(model.Password, BCryptNet.GenerateSalt(12));

            var user = _mapper.Map<User>(model);

            await _usersCollection.InsertOneAsync(user);
        }

        public async Task UpdateProfileAsync(string id, UpdateProfileRequest model)
        {
            var user = await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (user == null)
                throw new Exception("User not found.");

            var existingUser = await _usersCollection.Find(x => (x.Email == model.Email) && x.Id != id)
                .FirstOrDefaultAsync();

            if (existingUser != null)
                throw new Exception("Email is already in use by another user.");

            if (model.Password != model.ConfirmPassword)
                throw new Exception("Password and confirm password must be the same");

            var hashed = BCryptNet.HashPassword(model.Password, BCryptNet.GenerateSalt(12));

            if (hashed != user.Password)
                model.Password = hashed;

            _mapper.Map(model, user);

            await _usersCollection.ReplaceOneAsync(x => x.Id == id, user);
        }
        #endregion
    }
}
