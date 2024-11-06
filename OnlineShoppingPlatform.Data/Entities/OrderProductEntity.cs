
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingPlatform.Data.Entities
{
    // Represents the relationship between an order and a product, including quantity
    public class OrderProductEntity : BaseEntity
    {
        public int OrderId { get; set; }  // ID of the associated order
        public OrderEntity Order { get; set; }  // Navigation property for the order
        public int ProductId { get; set; }  // ID of the associated product
        public ProductEntity Product { get; set; }   // Navigation property for the product
        public int Quantity { get; set; }  // Quantity of the product in the order

    }
    // Configuration class for OrderProductEntity, defining relationships and constraints
    public class OrderProductConfiguration : BaseConfiguration<OrderProductEntity>
    {
        // Configures properties and relationships of OrderProductEntity
        public override void Configure(EntityTypeBuilder<OrderProductEntity> builder)
        {
            // Defines a composite primary key using OrderId and ProductId
            builder.Ignore(x => x.Id);
            builder.HasKey("OrderId", "ProductId");
            // Defines a foreign key relationship with ProductEntity
            builder.HasOne(x => x.Product)
                   .WithMany(x => x.OrderProducts)
                   .HasForeignKey(x => x.ProductId);
            // Defines a foreign key relationship with OrderEntity
            builder.HasOne(x => x.Order)
                   .WithMany(x => x.OrderProducts)
                   .HasForeignKey(x => x.OrderId);

            base.Configure(builder);
        }
    }
}

