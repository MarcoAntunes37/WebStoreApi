using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebStoreApi.Interfaces;
using WebStoreApi.Collections.ViewModels.Products.Register;
using WebStoreApi.Collections.ViewModels.Products.Update;

namespace WebStoreApi.Controllers
{
    
    [Route("api/products")]
    
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductsService _productService;

        public ProductController(IProductsService productService)
        {
            _productService = productService;
        }        
                
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var response = await _productService.GetAsync();

            if (response == null) return NotFound();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(string id)
        {            
            var response = await _productService.GetAsync(id);

            if (response == null) return NotFound();

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(RegisterProductRequest productDto )
        {
            await _productService.CreateAsync(productDto);

            return Ok("Product created successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, UpdateProductRequest updateProduct)
        {            
            await _productService.UpdateAsync(id, updateProduct);

            return Ok("Product updated successfully.");   
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            await _productService.RemoveAsync(id);

            return Ok("Product deleted successfully");
        }
    }
}
