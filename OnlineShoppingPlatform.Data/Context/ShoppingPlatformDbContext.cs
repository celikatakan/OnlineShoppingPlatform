using Microsoft.EntityFrameworkCore;
using OnlineShoppingPlatform.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingPlatform.Data.Context
{
    // DbContext for the shopping platform, responsible for database operations
    public class ShoppingPlatformDbContext : DbContext
    {
        // Constructor that accepts DbContextOptions for configuration
        public ShoppingPlatformDbContext(DbContextOptions<ShoppingPlatformDbContext> options) : base(options)
        {

        }
        // DbSet for managing entities
        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<ProductEntity> Products => Set<ProductEntity>();
        public DbSet<OrderEntity> Orders => Set<OrderEntity>();
        public DbSet<OrderProductEntity> OrderProducts => Set<OrderProductEntity>();
        public DbSet<SettingEntity> Settings { get; set; }

        // Configures the model for the database using Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            // Seed initial data for SettingEntity, setting maintenance mode to false
            modelBuilder.Entity<SettingEntity>().HasData(new SettingEntity
            {
                Id = 1,
                MaintenenceMode = false,
            });



            base.OnModelCreating(modelBuilder);
        }



    }
}
