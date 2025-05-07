using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Domain.Entities
{
    public class ProductImage
    {
        [Key]
        public Guid Id { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } 

        public Guid ProductId { get; set; }
        public Product? Product { get; set; }

        private ProductImage() { }

        public ProductImage(
            Guid id,
            string? imageUrl, 
            Guid productId)
        {
            Id = id;
            ImageUrl = imageUrl;
            CreatedAt = DateTime.UtcNow;
            ProductId = productId;
        }
    }
}
