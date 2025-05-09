using EcommerceApp.Application.DTOs.Category;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Domain.Constants.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Web.Controllers
{

    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;
        public CategoryController(
            ICategoryService categoryService,
            ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }
        [Authorize(Roles = UserRoles.SuperAdmin)]
        public async Task<IActionResult> Index()
        {
            CategoryPageDto categoryPageDto = new CategoryPageDto();

            var result = await _categoryService.GetAllCategoriesAsync();

            if (result.IsSuccess)
            {
                categoryPageDto.CategoryDtos = result.Value.ToList();
            }

            return View(categoryPageDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryPageDto dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _categoryService.CreateCategoryAsync(dto.CreateCategoryDto);
                    if (result.IsSuccess)
                    {
                        TempData["SuccessMessage"] = "Category created successfully!";
                        return RedirectToAction("Index");
                    }

                    TempData["InfoMessage"] = result.Error;
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occurred while creating category: {ex}");
                    TempData["ErrorMessage"] = "An unexpected error occurred while creating the category. Please try again.";
                    return RedirectToAction("Index");
                }
            }

            return View("Index", dto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _categoryService.DeleteCategoryAsync(id);
                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = result.Value;
                    return RedirectToAction("Index");
                }

                TempData["InfoMessage"] = result.Error;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while deleting category: {ex}");
                TempData["ErrorMessage"] = "An unexpected error occurred while deleting the category. Please try again.";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchTerm)
        {
            var categories = await _categoryService.SearchCategoriesByNameAsync(searchTerm);
            return Json(categories.Value);
        }
    }
}
