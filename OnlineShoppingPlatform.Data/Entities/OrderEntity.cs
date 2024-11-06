using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingPlatform.Data.Entities
{
    // Represents an order in the system
    public class OrderEntity : BaseEntity
    {
        public DateTime OrderDate { get; set; } // Date the order was placed
        public decimal TotalAmount { get; set; }  // Total amount of the order
        public int CustomerId { get; set; }  // ID of the customer who placed the order
        public UserEntity Customer { get; set; }    // Customer entity representing the user who placed the order
        public ICollection<OrderProductEntity> OrderProducts { get; set; }   // Collection of products in the order

    }
    // Configuration class for OrderEntity, specifying relationships and constraints
    public class OrderConfiguration : BaseConfiguration<OrderEntity>
    {
        // Configures the properties and relationships of OrderEntity
        public override void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            // Defines a foreign key relationship between OrderEntity and UserEntity
            builder.HasOne(x => x.Customer)
                   .WithMany(x => x.Orders)
                   .HasForeignKey(x => x.CustomerId);


            base.Configure(builder);
        }
    }
}
