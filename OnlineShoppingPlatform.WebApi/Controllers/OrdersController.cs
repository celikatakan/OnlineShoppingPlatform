using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingPlatform.Business.Operations.Order;
using OnlineShoppingPlatform.Business.Operations.Order.Dtos;
using OnlineShoppingPlatform.Business.Operations.Product.Dtos;
using OnlineShoppingPlatform.Business.Operations.Product;
using OnlineShoppingPlatform.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using OnlineShoppingPlatform.WebApi.Filters;

namespace OnlineShoppingPlatform.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        // Dependency injection for order and product services
        private readonly IOrderService _orderService;
        
        // Constructor to initialize OrdersController with order and product services
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
           
        }
        // Endpoint to retrieve an order by its ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderId(int id)
        {
            var order = await _orderService.GetOrderId(id);
            if (order == null)
                return NotFound();
            return
                Ok(order);
        }
        // Endpoint to retrieve all orders
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllOrders();
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder(AddOrderRequest request)
        {
            // Convert AddOrderRequest to AddOrderDto and send it to the order service
            var orderDto = await _orderService.AddOrder(new AddOrderDto
            {
                CustomerId = request.CustomerId,
                // Convert the list of products, mapping each to OrderProductDto
                Products = request.Products.Select(p => new OrderProductDto
                {
                    ProductId = p.ProductId,
                    Quantity = p.Quantity
                }).ToList()
            });
            // If the order was successful, return a 200 OK response
            if (orderDto.IsSucceed)         
                return Ok(orderDto);

            return BadRequest(orderDto.Message);

        }
        [HttpPut("{id}")]
        [TimeControlFilter]
        public async Task<IActionResult> UpdateOrder(int id, UpdateOrderRequest request)
        {
            // Map UpdateOrderRequest to UpdateOrderDto
            var updateOrderDto = new UpdateOrderDto
            {
                Id = id,
                CustomerId = request.CustomerId,
                Products = request.Products,
            };

            var result = await _orderService.UpdateOrder(updateOrderDto);

            if (!result.IsSucceed)
                return NotFound();
            else
                return await GetOrderId(id);
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [TimeControlFilter]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            // Delete the order and check result
            var result = await _orderService.DeleteOrder(id);
            if (result.IsSucceed)        
                return Ok();

            return BadRequest();
        }

    }
}
