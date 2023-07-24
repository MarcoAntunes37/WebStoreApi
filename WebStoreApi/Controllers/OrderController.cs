using Microsoft.AspNetCore.Mvc;
using WebStoreApi.Interfaces;
using WebStoreApi.Collections.ViewModels.Products.Register;
using WebStoreApi.Collections.ViewModels.Products.Update;
using WebStoreApi.Collections.ViewModels.Orders.Register;
using WebStoreApi.Collections.ViewModels.Orders.Update;

namespace WebStoreApi.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrdersService _orderService;

        public OrderController(IOrdersService orderService)
        {
            _orderService = orderService;
        }        
                
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var response = await _orderService.GetAsync();

            if (response == null) return NotFound();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(string id)
        {            

            var response = await _orderService.GetAsync(id);

            if (response == null) return NotFound();

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(RegisterOrderRequest orderDto)
        {
            await _orderService.CreateAsync(orderDto);

            return Ok("Order created successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(string id, UpdateOrderRequest updateOrder)
        {            
            await _orderService.UpdateAsync(id, updateOrder);

            return Ok("Order updated successfully.");   
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            await _orderService.RemoveAsync(id);

            return Ok("Order deleted successfully");
        }
    }
}
