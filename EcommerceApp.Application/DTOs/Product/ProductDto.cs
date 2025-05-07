using EcommerceApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.Application.DTOs.Product
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public Guid CategoryId { get; set; }
        public int StockQuantity { get; set; }
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    }
}
