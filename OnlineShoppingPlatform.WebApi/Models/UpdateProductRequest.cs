using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingPlatform.WebApi.Models
{
    public class UpdateProductRequest
    {
        [Required]
        public string ProductName { get; set; }
        [Required]
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
       
    }
}
