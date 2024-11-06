using OnlineShoppingPlatform.Business.Operations.Order.Dtos;
using OnlineShoppingPlatform.Business.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingPlatform.Business.Operations.Order
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetAllOrders();
        Task<OrderDto> GetOrderId(int id);      
        Task<ServiceMessage> UpdateOrder(UpdateOrderDto order);
        Task<ServiceMessage> DeleteOrder(int id);
        Task<ServiceMessage<AddOrderDto>> AddOrder(AddOrderDto order);
    }
}
