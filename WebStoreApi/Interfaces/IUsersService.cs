using WebStoreApi.Collections.ViewModels.Users.Authorization;
using WebStoreApi.Collections.ViewModels.Users.Register;
using WebStoreApi.Collections.ViewModels.Users.Update;
using WebStoreApi.Collections;

namespace WebStoreApi.Interfaces
{
    public interface IUsersService
    {
        Task<AuthenticateResponse> Login(AuthenticateRequest model);
        Task Logout(string id);
        Task<List<User>> GetAsync();
        Task<User> GetAsync(string id);
        Task CreateAsync(RegisterProfileRequest model);
        Task UpdateProfileAsync(string id, UpdateProfileRequest model);
        Task RemoveAsync(string id);
    }
}
