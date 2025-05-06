using EcommerceApp.Domain.Entities;

namespace EcommerceApp.Domain.IRepositories
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(Guid id);
        Task<IEnumerable<Category>> GetAllAsync(bool orderby);
        Task<IEnumerable<Category>> SearchAllByNameAsync(string searchTerm);
        Task<Category> AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsByNameAsync(string name);
    }
}
