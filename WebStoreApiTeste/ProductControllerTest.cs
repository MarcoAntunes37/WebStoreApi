using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using Xunit;
using WebStoreApi.Controllers;
using WebStoreApi.Collections;
using WebStoreApi.Interfaces;
using WebStoreApi.Collections.ViewModels.Products.Update;

namespace WebStoreApiTeste
{
    public class ProductControllerTest
    {
        private readonly ProductController _productsController;
        private readonly Mock<IProductsService> _productServiceMock = new Mock<IProductsService>();
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();

        public ProductControllerTest()
        {
            _productsController = new ProductController(_productServiceMock.Object);
        }

        #region getProducts
        [Fact]
        public async Task GetProdutcsAsync_ShouldReturnArrayOfProducts_WhenProductsExists()
        {
            List<Product> products = new List<Product>(){
                new Product{
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "giftcard",
                    Description = "amazon giftcard",
                    Price = 10.99,
                    ImageUrl = "",
                    Quantity = 1
                },
                 new Product{
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "giftcard",
                    Description = "psn giftcard",
                    Price = 10.99,
                    ImageUrl = "",
                    Quantity = 1
                },
                 new Product{
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "giftcard",
                    Description = "netflix giftcard",
                    Price = 10.99,
                    ImageUrl = "",
                    Quantity = 1
                },
            };

            _productServiceMock.Setup(x => x.GetAsync()).ReturnsAsync(products);

            var result = await _productsController.GetProducts();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetProductsAsync_ShouldReturnNotFound_WhenProductsNotExist()
        {
            List<Product> products = null;

            _productServiceMock.Setup(x => x.GetAsync()).ReturnsAsync(products);

            var result = await _productsController.GetProducts();

            Assert.IsType<NotFoundResult>(result);
        }
        #endregion

        #region getProductById
        [Fact]
        public async Task GetProductAsync_ShouldReturnAProduct_WhenProductExist()
        {
            var product = new Product
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "giftcard",
                Description = "amazon giftcard",
                Price = 10.99,
                ImageUrl = "",
                Quantity = 1
            };

            _productServiceMock.Setup(x => x.GetAsync(product.Id)).ReturnsAsync(product);

            var result = await _productsController.GetProduct(product.Id);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetProductAsync_ShouldReturnNotFound_WhenProductNotExist()
        {
            var productId = ObjectId.GenerateNewId().ToString();

            _productServiceMock.Setup(x => x.GetAsync(productId)).ReturnsAsync(() => null);

            var result = await _productsController.GetProduct(productId);

            Assert.IsType<NotFoundResult>(result);
        }
        #endregion

        #region updateProduct
        [Fact]
        public async Task UpdateProduct_ShouldReturnNoContent_WhenProductUpdated()
        {
            var routeId = ObjectId.GenerateNewId().ToString();

            var product = new UpdateProductRequest
            {                
                Name = "giftcard",
                Description = "amazon giftcard",
                Price = 10.99,
                ImageUrl = "",
                Quantity = 1
            };

            _productServiceMock.Setup(x => x.GetAsync(routeId)).ReturnsAsync(new Product { Id = routeId });

            _productServiceMock.Setup(x => x.UpdateAsync(routeId, product));

            var result = await _productsController.UpdateProduct(routeId, product);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnNotFound_WhenProductNotExist()
        {
            var routeId = ObjectId.GenerateNewId().ToString();

            var product = new UpdateProductRequest
            {
                Name = "giftcard",
                Description = "amazon giftcard",
                Price = 10.99,
                ImageUrl = "",
                Quantity = 1
            };

            _productServiceMock.Setup(x => x.GetAsync(routeId)).ReturnsAsync(() => null);

            var result = await _productsController.UpdateProduct(routeId, product);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnNotFound_WhenProductIdIsDifferentToRouteId()
        {
            var routeId = ObjectId.GenerateNewId().ToString();

            var product = new UpdateProductRequest
            {
                Name = "giftcard",
                Description = "amazon giftcard",
                Price = 10.99,
                ImageUrl = "",
                Quantity = 1
            };

            _productServiceMock.Setup(x => x.GetAsync(routeId)).ReturnsAsync(() => null);

            var result = await _productsController.UpdateProduct(routeId, product);

            Assert.IsType<OkObjectResult>(result);
        }

        #endregion

        #region deleteProduct
        [Fact]
        public async Task DeleteProduct_ShouldReturnBadRequest_WhenProductNotExists()
        {
            var routeId = ObjectId.GenerateNewId().ToString();

            _productServiceMock.Setup(x => x.RemoveAsync(routeId));

            var result = await _productsController.DeleteProduct(routeId);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnNoContent_WhenProductIsDeleted()
        {
            var routeId = ObjectId.GenerateNewId().ToString();

            _productServiceMock.Setup(x => x.RemoveAsync(routeId));

            var result = await _productsController.DeleteProduct(routeId);

            Assert.IsType<OkObjectResult>(result);
        }
        #endregion
    }
}
