using OnlineShoppingPlatform.Business.Operations.Order.Dtos;

namespace OnlineShoppingPlatform.WebApi.Models
{
    public class UpdateOrderRequest
    {
        public int CustomerId { get; set; }
        
        public List<OrderProductDto> Products { get; set; }

    }
}
