using EcommerceApp.Domain.Entities;

namespace EcommerceApp.Domain.IRepositories
{
    public interface IProductRepository
    {
        Task<Product?> GetAsync(Guid? id, string? slug = "");
        Task<bool> AnyAsync(Guid id);
        Task<IEnumerable<Product>> GetAllAsync(int number = 10);
        Task<IEnumerable<Product>> GetByCategoryIdAsync(Guid categoryId, int number = 0);
        Task<Product> AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsBySlugAsync(string slug);

        Task DeleteProductImagesByProductId(Guid productId);
        Task AddProductImagesByProductId(List<ProductImage> productImages);
    }
}
