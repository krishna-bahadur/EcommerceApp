using EcommerceApp.Domain.Entities;
using EcommerceApp.Domain.IRepositories;
using EcommerceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace EcommerceApp.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task AddProductImagesByProductId(List<ProductImage> productImages)
        {
            _context.ProductImages.AddRange(productImages);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AnyAsync(Guid id)
        {
            return await _context.Products
            .AnyAsync(x => x.Id == id);
        }

        public async Task DeleteAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                product.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteProductImagesByProductId(Guid productId)
        {
            var productImages = await _context.ProductImages.Where(x => x.ProductId == productId).ToListAsync();
            _context.ProductImages.RemoveRange(productImages);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsBySlugAsync(string slug)
        {
            return await _context.Products
            .AnyAsync(x => x.Slug == slug);
        }

        public async Task<IEnumerable<Product>> GetAllAsync(int number = 10)
        {
            return await _context.Products
                .Include(x => x.Images)
                .OrderByDescending(x => x.CreatedAt)
                .Take(number)
                .ToListAsync();
        }

        public async Task<Product?> GetAsync(Guid? id, string? slug = "")
        {
            if (id != null)
                return await _context.Products.Include(x => x.Images).Where(x => x.Id == id).FirstOrDefaultAsync();

            if (!string.IsNullOrEmpty(slug))
                return await _context.Products.Include(x => x.Images).Where(x => x.Slug == slug).FirstOrDefaultAsync();

            return null;
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(Guid categoryId, int number = 0)
        {
            if (number > 0)
                return await _context.Products
                    .Where(b => b.CategoryId == categoryId)
                    .Include(b => b.Images)
                    .OrderByDescending(x => x.CreatedAt)
                    .Take(number)
                    .ToListAsync();

            return await _context.Products
                .Where(b => b.CategoryId == categoryId)
                .Include(b => b.Images)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
