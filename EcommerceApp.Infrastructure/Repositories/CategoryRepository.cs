using EcommerceApp.Domain.Entities;
using EcommerceApp.Domain.IRepositories;
using EcommerceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Category> AddAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task DeleteAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                category.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Categories
                .AnyAsync(b => b.Name == name);
        }

        public async Task<IEnumerable<Category>> GetAllAsync(bool orderby)
        {
            IQueryable<Category> query = _context.Categories;

            if (orderby)
            {
                query = query.OrderByDescending(c => c.CreatedAt);
            }

            return await query.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Category>> SearchAllByNameAsync(string searchTerm)
        {
            return await _context.Categories
                .Where(c => c.Name.ToLower().Contains(searchTerm))
                .ToListAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
