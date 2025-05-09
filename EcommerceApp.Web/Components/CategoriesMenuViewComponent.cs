using EcommerceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Web.Components
{
    public class CategoriesMenuViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public CategoriesMenuViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var response = await _categoryService.GetAllCategoriesAsync();
            return View(response.Value);
        }
    }
}
