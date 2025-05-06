namespace EcommerceApp.Domain.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
