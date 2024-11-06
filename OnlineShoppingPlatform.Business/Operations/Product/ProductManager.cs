using Microsoft.EntityFrameworkCore;
using OnlineShoppingPlatform.Business.Operations.Product.Dtos;
using OnlineShoppingPlatform.Business.Types;
using OnlineShoppingPlatform.Data.Entities;
using OnlineShoppingPlatform.Data.Repository;
using OnlineShoppingPlatform.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingPlatform.Business.Operations.Product
{
    // ProductManager class implements IProductService and manages product-related operations
    public class ProductManager : IProductService
    { 
        private readonly IUnitOfWork _unitOfWork; // Unit of Work for handling database transactions
        private readonly IRepository<ProductEntity> _repository; // Repository for ProductEntity

        // Constructor to inject dependencies
        public ProductManager(IUnitOfWork unitOfWork, IRepository<ProductEntity> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }
        // Updates the price of a product identified by its ID
        public async Task<ServiceMessage> UpdateJustPrice(int id, int changeByPrice)
        {
            var product = _repository.GetById(id);
            if (product == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "There is no product compatible with this ID.",
                };
            }
            // Update the product price
            product.Price = changeByPrice;

            _repository.Update(product);
            try
            {
                await _unitOfWork.SaveChangesAsync(); // Save changes to the database
            }
            catch (Exception)
            {

                throw new Exception("An error occurred while changing the product price.");
            }

            return new ServiceMessage
            {
                IsSucceed = true,
            };
        }
        // Adds a new product to the repository
        public async Task<ServiceMessage> AddProduct(AddProductDto product)
        {
            // Check if a product with the same name already exists
            var hasProduct = _repository.GetAll(x => x.ProductName.ToLower() == product.ProductName.ToLower()).Any();

            if (hasProduct)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Product already available."
                };
            }
            // Create a new product entity
            var productEntity = new ProductEntity
            {
                ProductName = product.ProductName,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
            };
            _repository.Add(productEntity);
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("An unexpected error occurred.");
            }
            return new ServiceMessage
            {
                IsSucceed = true,
            };

        }
        // Deletes a product identified by its ID
        public async Task<ServiceMessage> DeleteProduct(int id)
        {
            // Retrieve the product entity by ID
            var product = _repository.GetById(id);

            if (product == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "The product to be deleted could not be found."
                };
            }
            _repository.Delete(id);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw new Exception("An error was encountered while deleting the product.");
            }

            return new ServiceMessage
            {
                IsSucceed = true
            };
        }
        // Retrieves a product by its ID
        public async Task<ProductDto> GetProduct(int id)
        {
            var product = await _repository.GetAll(x => x.Id == id)
                 .Select(x => new ProductDto
                 {
                     Id = x.Id,
                     ProductName = x.ProductName,
                     Price = x.Price,
                     StockQuantity = x.StockQuantity,
                 }).FirstOrDefaultAsync();

            return product;
        }
        // Retrieves all products
        public async Task<List<ProductDto>> GetProducts()
        {
            var products = await _repository.GetAll()
                .Select(x => new ProductDto
                {
                    Id = x.Id,
                    ProductName = x.ProductName,
                    Price = x.Price,
                    StockQuantity = x.StockQuantity,
                }).ToListAsync();
            return products;
        }
        // Updates an existing product's details
        public async Task<ServiceMessage> UpdateProduct(UpdateProductDto product)
        {
            var productEntity = _repository.GetById(product.Id);

            if (productEntity == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Product not found."
                };
            }
            // Update product details
            productEntity.ProductName = product.ProductName;
            productEntity.Price = product.Price;
            productEntity.StockQuantity = product.StockQuantity;

            _repository.Update(productEntity);
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("An error was encountered while updating product information.");
            }
            return new ServiceMessage
            {
                IsSucceed = true
            };
        }


    }
}
