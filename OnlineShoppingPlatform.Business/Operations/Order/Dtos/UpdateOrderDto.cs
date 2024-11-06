using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingPlatform.Business.Operations.Order.Dtos
{
    public class UpdateOrderDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public List<OrderProductDto> Products { get; set; }
    }
}
