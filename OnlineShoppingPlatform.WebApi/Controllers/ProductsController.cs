using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingPlatform.Business.Operations.Product;
using OnlineShoppingPlatform.Business.Operations.Product.Dtos;
using OnlineShoppingPlatform.WebApi.Filters;
using OnlineShoppingPlatform.WebApi.Models;

namespace OnlineShoppingPlatform.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {   
        // Dependency injection for the product service
        private readonly IProductService _productService;
        
        // Constructor to initialize ProductsController with the product service
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        // Endpoint to retrieve a product by its ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetId(int id)
        {
            var product = await _productService.GetProduct(id);
            if (product == null)
                return NotFound();
            else
                return Ok(product);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetProducts();
            return Ok(products);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct(AddProductRequest request)
        {
            // Map AddProductRequest to AddProductDto
            var addProductDto = new AddProductDto
            {
                ProductName = request.ProductName,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
            };
            var result = await _productService.AddProduct(addProductDto);

            if (result.IsSucceed)
                return Ok();
            else
                return BadRequest(result.Message);
        }
        // Endpoint to update only the price of a product by ID (Admin role required)
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateJustPrice(int id, int changeByPrice)
        {
            // Update product price and check result
            var result = await _productService.UpdateJustPrice(id, changeByPrice);
            if (!result.IsSucceed)
                return NotFound(result.Message);
            else
                return Ok();
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [TimeControlFilter]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductRequest request)
        {
            // Map UpdateProductRequest to UpdateProductDto
            var updateProductDto = new UpdateProductDto
            {
                Id = id,
                ProductName = request.ProductName,
                Price = request.Price,
                StockQuantity = request.StockQuantity,       
            };
 
            var result = await _productService.UpdateProduct(updateProductDto);

            if (!result.IsSucceed)
                return BadRequest(result.Message);
            else
                return await GetId(id);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [TimeControlFilter]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            // Delete the product and check result
            var result = await _productService.DeleteProduct(id);

            if (!result.IsSucceed)
                return NotFound(result.Message);
            else
                return Ok();
        }
    }
}
