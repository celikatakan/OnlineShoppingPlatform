using Microsoft.EntityFrameworkCore;
using OnlineShoppingPlatform.Business.Operations.Order.Dtos;
using OnlineShoppingPlatform.Business.Operations.Product;
using OnlineShoppingPlatform.Business.Operations.Product.Dtos;
using OnlineShoppingPlatform.Business.Types;
using OnlineShoppingPlatform.Data.Entities;
using OnlineShoppingPlatform.Data.Repository;
using OnlineShoppingPlatform.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingPlatform.Business.Operations.Order
{
    // OrderManager class implements IOrderService and manages order-related operations
    public class OrderManager : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<OrderEntity> _orderRepository;
        private readonly IRepository<OrderProductEntity> _orderProductRepository;
        private readonly IProductService _productService;
        // Constructor to inject dependencies
        public OrderManager(IUnitOfWork unitOfWork, IRepository<OrderEntity> orderRepository, IRepository<OrderProductEntity> orderProductRepository, IProductService productService)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
            _orderProductRepository = orderProductRepository;
            _productService = productService;
        }
        // Adds a new order along with its associated products
        public async Task<ServiceMessage<AddOrderDto>> AddOrder(AddOrderDto order)
        {
            var hasUsers = await _orderRepository.UserExistAsync(order.CustomerId);
            if (!hasUsers)
            {
                return new ServiceMessage<AddOrderDto>
                {
                    IsSucceed = false,
                    Message = "Any user is not found."

                };
            }
            // Begin a new database transaction
            await _unitOfWork.BeginTransaction();
            // Calculate total amount for the order
            decimal totalAmount = 0;
            foreach (var item in order.Products)
            {
                // Get product details and calculate total price for each item
                ProductDto product = await _productService.GetProduct(item.ProductId);

                if(product is null)
                {
                    await _unitOfWork.RollBackTransaction();
                    return new ServiceMessage<AddOrderDto>
                    {
                        IsSucceed = false,
                        Message = "Product not found."
                    };
                }
                totalAmount += product.Price * item.Quantity;
            }
            // Create a list of OrderProductDto for the order
            List<OrderProductDto> orderProductDto = new List<OrderProductDto>();
            foreach (var item in order.Products)
            {
                orderProductDto.Add(new OrderProductDto()
                {
                    Quantity = item.Quantity,
                    ProductId = item.ProductId
                });
            }

            // Create a new order entity
            var newOrder = new OrderEntity
            {

                OrderDate = DateTime.Now,
                TotalAmount = totalAmount,
                CustomerId = order.CustomerId,

            };
            // Add the order to the repository
            _orderRepository.Add(newOrder);

            try
            {
                await _unitOfWork.SaveChangesAsync();   // Save changes to the database
            }
            catch (Exception e)
            {

                throw new Exception("An unexpected error occurred." + e.Message);
            }
            // Add associated products to the order
            foreach (var productIds in order.Products)
            {
                var orderProduct = new OrderProductEntity()
                {
                    OrderId = newOrder.Id,
                    ProductId = productIds.ProductId,
                    Quantity = productIds.Quantity

                };

                _orderProductRepository.Add(orderProduct);
            }
            try
            {
                // Save changes to the database and commit the transaction
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackTransaction();
                throw new Exception("An unexpected error occurred.");
            }
            return new ServiceMessage<AddOrderDto>
            {
                IsSucceed = true,
                Message = "Order added successfully.",
                Data = new AddOrderDto
                {
       
                    CustomerId = order.CustomerId,
                    Products = orderProductDto
                }
            };
        }
        // Deletes an existing order by ID
        public async Task<ServiceMessage> DeleteOrder(int id)
        {
            var order = _orderRepository.GetById(id); // Retrieve the order entity by ID
            if (order == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "The order to be deleted could not be found."
                };
            }
            // Delete the order from the repository
            _orderRepository.Delete(id);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw new Exception("An error was encountered while deleting the order.");
            }

            return new ServiceMessage
            {
                IsSucceed = true
            };
        }
        // Retrieves all orders with their details
        public async Task<List<OrderDto>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAll()
                   .Select(x => new OrderDto
                   {
                       Id = x.Id,
                       OrderDate = x.OrderDate,
                       TotalAmount = x.TotalAmount,
                       // Mapping customer information
                       Customer = new OrderCustomerDto
                       {
                           Id = x.Customer.Id,
                           FirstName = x.Customer.FirstName,
                           LastName = x.Customer.LastName,
                           Email = x.Customer.Email,
                           PhoneNumber = x.Customer.PhoneNumber
                       },
                       // Mapping products associated with the order
                       Products = x.OrderProducts.Select(p => new Dtos.ProductInfoDto
                       {
                           ProductName = p.Product.ProductName,
                           Quantity = p.Quantity
                       }).ToList()

                   }).ToListAsync();
            return orders;
        }
        // Retrieves a specific order by its ID
        public async Task<OrderDto> GetOrderId(int id)
        {

            var order = await _orderRepository.GetAll(x => x.Id == id)
                     .Select(x => new OrderDto
                     {
                         Id = x.Id,
                         OrderDate = x.OrderDate,
                         TotalAmount = x.TotalAmount,
                         // Mapping customer information
                         Customer = new OrderCustomerDto
                         {
                             Id = x.Customer.Id,
                             FirstName = x.Customer.FirstName,
                             LastName = x.Customer.LastName,
                             Email = x.Customer.Email,
                             PhoneNumber = x.Customer.PhoneNumber
                         },
                         // Mapping products associated with the order
                         Products = x.OrderProducts.Select(p => new Dtos.ProductInfoDto
                         {
                             ProductName = p.Product.ProductName,
                             Quantity = p.Quantity
                         }).ToList()

                     }).FirstOrDefaultAsync();

            return order;
        }
        // Updates an existing order's details
        public async Task<ServiceMessage> UpdateOrder(UpdateOrderDto order)
        {
            // Retrieve the existing order entity by ID
            var orderEntity = _orderRepository.GetById(order.Id);

            if (orderEntity is null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Order not found."
                };
            }

            await _unitOfWork.BeginTransaction();
            // Update order details
            orderEntity.CustomerId = order.CustomerId;
            _orderRepository.Update(orderEntity);


            try
            {
                await _unitOfWork.SaveChangesAsync();    // Save changes to the database
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackTransaction();
                throw new Exception("An error was encountered while updating order information.");
            }

            var orderProducts = _orderProductRepository.GetAll(x => x.OrderId == orderEntity.Id).ToList();

            foreach (var product in orderProducts)
            {
                _orderProductRepository.Delete(product, false);
            }

            // Add updated products to the order
            foreach (var productDto in order.Products)
            {
                var orderProduct = new OrderProductEntity
                {
                    ProductId = productDto.ProductId,
                    Quantity = productDto.Quantity,
                    OrderId = orderEntity.Id,
                };
                _orderProductRepository.Add(orderProduct);
            }
            try
            {
                // Save changes to the database and commit the transaction
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();

            }
            catch (Exception)
            {
                await _unitOfWork.RollBackTransaction();
                throw new Exception("An error was encountered while updating order information.");
            }
            return new ServiceMessage
            {
                IsSucceed = true,               
            };
        }
    }
}
