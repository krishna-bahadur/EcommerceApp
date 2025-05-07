using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.Domain.Entities
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();

        private Product() { }

        public Product(
            Guid id,
            string? name, 
            string? slug, 
            string? description, 
            decimal? price, 
            decimal? discountPrice, 
            int? stockQuantity, 
            Guid? categoryId,
            List<ProductImage> images)
        {
            Id = id;
            Name = name;
            Slug = slug;
            Description = description;
            Price = (Decimal) price!;
            DiscountPrice = discountPrice;
            StockQuantity = (int) stockQuantity!;
            IsActive = true;
            IsDeleted = false;
            CreatedAt = DateTime.UtcNow;
            CategoryId = (Guid) categoryId!;
            Images = images;
        }
    }
}
