using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingPlatform.Data.Entities
{
    // Base class containing common properties for entities
    public class BaseEntity
    {
        public int Id { get; set; } // Primary key for the entity
        public DateTime CreatedDate { get; set; }  // Date the entity was created
        public DateTime? ModifiedDate { get; set; }  // Date the entity was last modified, nullable
        public bool IsDeleted { get; set; }  // Soft delete flag to indicate if the entity is deleted
    }
    // Base configuration class for entity types, providing a common query filter
    public abstract class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
    {
        // Configures entity properties and behavior
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            // Applies a global filter to exclude soft-deleted entities in queries
            builder.HasQueryFilter(x => x.IsDeleted == false);

        }
    }
}
