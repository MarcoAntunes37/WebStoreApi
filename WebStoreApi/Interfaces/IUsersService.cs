using WebStoreApi.Collections.ViewModels.Users.Authorization;
using WebStoreApi.Collections.ViewModels.Users.Register;
using WebStoreApi.Collections.ViewModels.Users.Update;
using WebStoreApi.Collections;
using Microsoft.OpenApi.Any;

namespace WebStoreApi.Interfaces
{
    public interface IUsersService
    {
        Task<AuthenticateResponse> Login(AuthenticateRequest model);       
        Task<List<User>> GetAsync();
        Task<User> GetAsync(string id);
        Task CreateAsync(RegisterProfileRequest model);
        Task UpdateProfileAsync(string id, UpdateProfileRequest model);
        Task<User> GetByUsernameAsync(string username);
        Task RemoveAsync(string id);
        Task UpdatePasswordAsync(string userId, UpdatePasswordRequest model);
        Task InsertAddress(string userId, RegisterAddressRequest model);
        Task UpdateAddress(string userId, UpdateAddressRequest model);
        Task DeleteAddress(string userId, string addressId);
        Task InsertCreditCard(string userId, RegisterCreditCardRequest model);       
        Task UpdateCreditCard(string userId, UpdateCreditCardRequest model);
        Task DeleteCreditCard(string userId, string creditCardId);
        Task InsertShoppingCartItem(string userId, RegisterCartItemsRequest model);
        Task UpdateShoppingCartItem(string userId, UpdateCartItemRequest model);
        Task DeleteShoppingCartItem(string userId, string cartItemId);
    }
}
