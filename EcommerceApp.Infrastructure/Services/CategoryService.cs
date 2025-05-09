using EcommerceApp.Application.Common.Results;
using EcommerceApp.Application.DTOs.Category;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Domain.Entities;
using EcommerceApp.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<CategoryDto>> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            if (await _categoryRepository.ExistsByNameAsync(createCategoryDto.Name))
            {
                return Result<CategoryDto>.Failure($"Category with name {createCategoryDto.Name} already exists.");
            }

            var category = new Category(Guid.NewGuid(), createCategoryDto.Name);
            await _categoryRepository.AddAsync(category);

            var categoryDto = MapToDto(category);

            return Result<CategoryDto>.Success(categoryDto);
        }

        public async Task<Result<string>> DeleteCategoryAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return Result<string>.Failure($"Category with Id {id} not found");
            }
            
            await _categoryRepository.DeleteAsync(id);

            return Result<string>.Success("Category deleted successfully.");
        }

        public async Task<Result<IEnumerable<CategoryDto>>> GetAllCategoriesAsync(bool orderby = true)
        {
            var categories = await _categoryRepository.GetAllAsync(orderby);

            var categoryDtos = categories.Select(MapToDto);

            return Result<IEnumerable<CategoryDto>>.Success(categoryDtos);
        }

        public async Task<Result<CategoryDto>> GetCategoryByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return Result<CategoryDto>.Failure($"Category with Id {id} not found");
            }

            var categoryDto = MapToDto(category);

            return Result<CategoryDto>.Success(categoryDto);
        }
        

        private static CategoryDto MapToDto(Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
            };
        }

        public async Task<Result<IEnumerable<CategoryDto>>> SearchCategoriesByNameAsync(string searchTerm)
        {
            var categories = await _categoryRepository.SearchAllByNameAsync(searchTerm);

            var categoryDtos = categories.Select(MapToDto);

            return Result<IEnumerable<CategoryDto>>.Success(categoryDtos);
        }
    }
}