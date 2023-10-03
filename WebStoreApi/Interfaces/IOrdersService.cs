using WebStoreApi.Collections;
using WebStoreApi.Collections.ViewModels.Orders.Register;
using WebStoreApi.Collections.ViewModels.Orders.Update;

namespace WebStoreApi.Interfaces
{
    public interface IOrdersService
    {
        Task<List<Order>> GetAsync();
        Task<List<Order>> GetAsyncByUserId(string userId);
        Task<Order> GetAsync(string id);
        Task CreateAsync(RegisterOrderRequest model);
        Task UpdateAsync(string id, UpdateOrderRequest model);
        Task RemoveAsync(string id);
    }
}
