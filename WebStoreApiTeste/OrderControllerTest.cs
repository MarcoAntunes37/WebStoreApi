using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using Xunit;
using WebStoreApi.Controllers;
using WebStoreApi.Collections;
using WebStoreApi.Interfaces;
using WebStoreApi.Collections.ViewModels.Orders;
using WebStoreApi.Collections.ViewModels.Orders.Update;

namespace WebStoreApiTeste
{
    public class OrderControllerTest
    {
        private readonly OrderController _ordersController;
        private readonly Mock<IOrdersService> _ordersServiceMock = new Mock<IOrdersService>();
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();

        public OrderControllerTest()
        {
            _ordersController = new OrderController(_ordersServiceMock.Object);
        }

        #region getOrders
        [Fact]
        public async Task GetOrdersAsyncShouldReturnArrayOfOrdersWhenOrdersExists()
        {
            List<Order> orders = new List<Order>(){
                new Order{
                    Id = ObjectId.GenerateNewId().ToString(),
                    UserId = ObjectId.GenerateNewId().ToString(),
                    OrderStatus = "Waiting for payment",
                    OrderDate = DateTime.UtcNow,
                    OrderItems = new List<OrderItem>()
                    {
                        new OrderItem
                        {
                            ProductId = ObjectId.GenerateNewId().ToString(),
                            ProductName = "aaaa",
                            Quantity = 1,
                            Price= 100
                        },
                        new OrderItem
                        {
                            ProductId = ObjectId.GenerateNewId().ToString(),
                            ProductName = "bbb",
                            Quantity = 1,
                            Price= 100
                        },
                        new OrderItem
                        {
                            ProductId = ObjectId.GenerateNewId().ToString(),
                            ProductName = "bcc",
                            Quantity = 1,
                            Price= 100
                        }
                    },
                    TotalPrice = 300
                },
                new Order{
                    Id = ObjectId.GenerateNewId().ToString(),
                    UserId = ObjectId.GenerateNewId().ToString(),
                    OrderStatus = "Waiting for return",
                    OrderDate = DateTime.UtcNow,
                    OrderItems = new List<OrderItem>()
                    {
                        new OrderItem
                        {
                            ProductId = ObjectId.GenerateNewId().ToString(),
                            ProductName = "ddd",
                            Quantity = 1,
                            Price= 100
                        },
                        new OrderItem
                        {
                            ProductId = ObjectId.GenerateNewId().ToString(),
                            ProductName = "eee",
                            Quantity = 1,
                            Price= 100
                        },
                        new OrderItem
                        {
                            ProductId = ObjectId.GenerateNewId().ToString(),
                            ProductName = "fff",
                            Quantity = 1,
                            Price= 100
                        }
                    },
                    TotalPrice = 300
                },
            };       

            _ordersServiceMock.Setup(x => x.GetAsync()).ReturnsAsync(orders);

            var result = await _ordersController.GetOrders();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetOrdersAsync_ShouldReturnNotFound_WhenOrdersNotExist()
        {
            List<Order> orders = null;

            _ordersServiceMock.Setup(x => x.GetAsync()).ReturnsAsync(orders);

            var result = await _ordersController.GetOrders();

            Assert.IsType<NotFoundResult>(result);
        }
        #endregion

        #region getOrderById
        [Fact]
        public async Task GetOrderAsync_ShouldReturnAOrder_WhenOrderExist()
        {
            var order = new Order(){
                Id = ObjectId.GenerateNewId().ToString(),
                UserId = ObjectId.GenerateNewId().ToString(),
                OrderStatus = "Waiting for payment",
                OrderDate = DateTime.UtcNow,
                OrderItems = new List<OrderItem>()
                {
                    new OrderItem
                    {
                        ProductId = ObjectId.GenerateNewId().ToString(),
                        ProductName = "aaaa",
                        Quantity = 1,
                        Price= 100
                    },
                },
                TotalPrice = 100                
            };

            _ordersServiceMock.Setup(x => x.GetAsync(order.Id)).ReturnsAsync(order);

            var result = await _ordersController.GetOrder(order.Id);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetOrderAsync_ShouldReturnNotFound_WhenOrderNotExist()
        {
            var orderId = ObjectId.GenerateNewId().ToString();

            _ordersServiceMock.Setup(x => x.GetAsync(orderId)).ReturnsAsync(() => null);

            var result = await _ordersController.GetOrder(orderId);

            Assert.IsType<NotFoundResult>(result);
        }
        #endregion

        #region updateOrder
        [Fact]
        public async Task UpdateOrder_ShouldReturnNoContent_WhenOrderUpdated()
        {
            var routeId = ObjectId.GenerateNewId().ToString();

            var order = new UpdateOrderRequest
            {
                UserId = ObjectId.GenerateNewId().ToString(),
                OrderStatus = "Waiting for payment",
                OrderDate = DateTime.UtcNow,
                OrderItems = new List<OrderItem>()
                {
                    new OrderItem
                    {
                        ProductId = ObjectId.GenerateNewId().ToString(),
                        ProductName = "aaaa",
                        Quantity = 1,
                        Price= 100
                    },
                },
                TotalPrice = 100
            };

            _ordersServiceMock.Setup(x => x.GetAsync(routeId)).ReturnsAsync(new Order { Id = routeId });

            _ordersServiceMock.Setup(x => x.UpdateAsync(routeId, order));

            var result = await _ordersController.UpdateOrder(routeId, order);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateOrder_ShouldReturnNotFound_WhenOrderNotExist()
        {
            var routeId = ObjectId.GenerateNewId().ToString();

            var order = new UpdateOrderRequest
            {
                UserId = ObjectId.GenerateNewId().ToString(),
                OrderStatus = "Waiting for payment",
                OrderDate = DateTime.UtcNow,
                OrderItems = new List<OrderItem>()
                {
                    new OrderItem
                    {
                        ProductId = ObjectId.GenerateNewId().ToString(),
                        ProductName = "aaaa",
                        Quantity = 1,
                        Price= 100
                    },
                },
                TotalPrice = 100
            };

            _ordersServiceMock.Setup(x => x.GetAsync(routeId)).ReturnsAsync(() => null);

            var result = await _ordersController.UpdateOrder(routeId, order);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateOrder_ShouldReturnNotFound_WhenOrderIdIsDifferentToRouteId()
        {
            var routeId = ObjectId.GenerateNewId().ToString();

            var order = new UpdateOrderRequest
            {
                UserId = ObjectId.GenerateNewId().ToString(),
                OrderStatus = "Waiting for payment",
                OrderDate = DateTime.UtcNow,
                OrderItems = new List<OrderItem>()
                {
                    new OrderItem
                    {
                        ProductId = ObjectId.GenerateNewId().ToString(),
                        ProductName = "aaaa",
                        Quantity = 1,
                        Price= 100
                    },
                },
                TotalPrice = 100
            };

            _ordersServiceMock.Setup(x => x.GetAsync(routeId)).ReturnsAsync(() => null);

            var result = await _ordersController.UpdateOrder(routeId, order);

            Assert.IsType<OkObjectResult>(result);
        }

        #endregion

        #region deleteOrder
        [Fact]
        public async Task DeleteOrder_ShouldReturnBadRequest_WhenOrderNotExists()
        {
            var routeId = ObjectId.GenerateNewId().ToString();

            _ordersServiceMock.Setup(x => x.RemoveAsync(routeId));

            var result = await _ordersController.DeleteOrder(routeId);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteOrder_ShouldReturnNoContent_WhenOrderIsDeleted()
        {
            var routeId = ObjectId.GenerateNewId().ToString();

            _ordersServiceMock.Setup(x => x.RemoveAsync(routeId));

            var result = await _ordersController.DeleteOrder(routeId);

            Assert.IsType<OkObjectResult>(result);
        }
        #endregion
    }
}
