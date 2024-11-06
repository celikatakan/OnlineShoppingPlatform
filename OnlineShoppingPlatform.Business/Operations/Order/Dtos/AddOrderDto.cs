using OnlineShoppingPlatform.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingPlatform.Business.Operations.Order.Dtos
{
    public class AddOrderDto
    {
        public int CustomerId { get; set; }
        public List<OrderProductDto> Products { get; set; }
    }
}
