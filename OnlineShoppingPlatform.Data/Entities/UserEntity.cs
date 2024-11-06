using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShoppingPlatform.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingPlatform.Data.Entities
{
    public class UserEntity : BaseEntity
    {
        public string FirstName { get; set; }  // First name of the user
        public string LastName { get; set; }   // Last name of the user
        public string Email { get; set; }   // Email address of the user, used as a unique identifier
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public UserType UserType { get; set; }
        public ICollection<OrderEntity> Orders { get; set; }

    }
    // Configuration for UserEntity, to define constraints and relationships
    public class UserConfiguration : BaseConfiguration<UserEntity>
    {
        public override void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            // Sets the FirstName as required and limits its length to 30 characters
            builder.Property(x => x.FirstName)
                   .IsRequired()
                   .HasMaxLength(30);
            // Sets the LastName as required and limits its length to 30 characters
            builder.Property(x => x.LastName)
                  .IsRequired()
                  .HasMaxLength(30);
            // Creates a unique index on Email to ensure no duplicate email addresses
            builder.HasIndex(x => x.Email)
                   .IsUnique();

            base.Configure(builder);
        }
    }
}
