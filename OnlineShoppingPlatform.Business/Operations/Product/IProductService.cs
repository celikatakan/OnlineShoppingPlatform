using OnlineShoppingPlatform.Business.Operations.Product.Dtos;
using OnlineShoppingPlatform.Business.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingPlatform.Business.Operations.Product
{
    public interface IProductService
    {
        Task<ProductDto> GetProduct(int id);
        Task<List<ProductDto>> GetProducts();
        Task<ServiceMessage> AddProduct(AddProductDto product);
        Task<ServiceMessage> UpdateJustPrice(int id, int changeByPrice);
        Task<ServiceMessage> UpdateProduct(UpdateProductDto product);
        Task <ServiceMessage> DeleteProduct(int id);     
    }
}
