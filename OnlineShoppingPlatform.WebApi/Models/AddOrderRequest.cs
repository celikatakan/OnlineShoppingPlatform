using OnlineShoppingPlatform.Business.Operations.Order.Dtos;
using OnlineShoppingPlatform.Data.Entities;
using System.Runtime.CompilerServices;

namespace OnlineShoppingPlatform.WebApi.Models
{
    public class AddOrderRequest
    {
        public int CustomerId { get; set; }
        public List<OrderProductDto> Products { get; set; }

    }
}
