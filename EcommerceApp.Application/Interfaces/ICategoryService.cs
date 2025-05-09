using EcommerceApp.Application.Common.Results;
using EcommerceApp.Application.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<Result<CategoryDto>> GetCategoryByIdAsync(Guid id);
        Task<Result<IEnumerable<CategoryDto>>> GetAllCategoriesAsync(bool orderby = true);
        Task<Result<CategoryDto>> CreateCategoryAsync(CreateCategoryDto categoryCreateDto);
        Task<Result<IEnumerable<CategoryDto>>> SearchCategoriesByNameAsync(string searchTerm);
        Task<Result<string>> DeleteCategoryAsync(Guid id);
    }
}
