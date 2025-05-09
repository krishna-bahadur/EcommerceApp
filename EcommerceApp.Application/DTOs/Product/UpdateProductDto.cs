using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.Application.DTOs.Product
{
    public class UpdateProductDto
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Product Name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "The {0} must be between {2} and {1} characters long.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Product Description is required.")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Product Price is required.")]
        public decimal? Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        [Required(ErrorMessage = "Stock Quantity is required.")]
        public int? StockQuantity { get; set; }
        [Required(ErrorMessage = "Category Id is required.")]
        public Guid? CategoryId { get; set; }
        public List<IFormFile>? Images { get; set; }
        public List<string> ExistingImages { get; set; } = new();
        public List<string> RemovedImages { get; set; } = new();
    }
}
