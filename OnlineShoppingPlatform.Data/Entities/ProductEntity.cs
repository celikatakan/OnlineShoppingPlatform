using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingPlatform.Data.Entities
{
    public class ProductEntity : BaseEntity
    {
        public string ProductName { get; set; }  // Name of the product
        public decimal Price { get; set; }  // Price of the product

        // Quantity of the product available in stock
        public int StockQuantity { get; set; }

        // Navigation property for the orders containing this product
        public ICollection<OrderProductEntity> OrderProducts { get; set; }
    }
      // Configuration class for ProductEntity, allows customization of model behavior
    public class ProductConfiguration : BaseConfiguration<ProductEntity>
    {
        // Configures additional properties or relationships for ProductEntity
        public override void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            base.Configure(builder);  // Calls base configuration to apply any global configurations like soft delete filter
        }
    }
}
