using EcommerceApp.Application.DTOs.Product;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Domain.Constants.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace EcommerceApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public ProductController(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        [Authorize(Roles = UserRoles.SuperAdmin)]
        public async Task<IActionResult> Index()
        {
            var result = await _productService.GetAllBlogsAsync(10);
            if (result.IsSuccess)
            {
                return View(result.Value);

            }

            return View();
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.SuperAdmin)]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            if (categories.IsSuccess)
            {
                ViewBag.Categories = new SelectList(categories.Value, "Id", "Name");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserRoles.SuperAdmin)]
        public async Task<IActionResult> Create(CreateProductDto dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _productService.CreateProductAsync(dto);
                    if (response.IsSuccess)
                    {
                        TempData["SuccessMessage"] = response.Value;
                        return View();
                    }

                    TempData["InfoMessage"] = response.Error;
                    return View(dto);
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "An unexpected error occurred while creating the category. Please try again.";
                    return RedirectToAction("Index");
                }
            }

            var categories = await _categoryService.GetAllCategoriesAsync();
            if (categories.IsSuccess)
            {
                ViewBag.Categories = new SelectList(categories.Value, "Id", "Name");
            }

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var response = await _productService.DeleteProductAsync(id);
                if (response.IsSuccess)
                {
                    TempData["SuccessMessage"] = response.Value;
                    return RedirectToAction("Index");
                }

                TempData["InfoMessage"] = response.Error;
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while deleting the product. Please try again.";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var response = await _productService.GetProductByIdAsync(id);

            if (response.IsSuccess)
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                if (categories.IsSuccess)
                {
                    ViewBag.Categories = new SelectList(categories.Value, "Id", "Name");
                }
                return View(response.Value);
            }

            TempData["InfoMessage"] = response.Error;
            return RedirectToAction("Index");
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(UpdateBlogDto dto, string action)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            if (action == "draft")
        //            {
        //                var response = await _blogService.UpdateBlogAsync(dto, true);
        //                if (response.IsSuccess)
        //                {
        //                    TempData["SuccessMessage"] = response.Value;
        //                    return RedirectToAction("Index");
        //                }

        //                TempData["InfoMessage"] = response.Error;
        //                return RedirectToAction("Index");
        //            }
        //            else if (action == "publish")
        //            {

        //                var response = await _blogService.UpdateBlogAsync(dto, false);
        //                if (response.IsSuccess)
        //                {
        //                    TempData["SuccessMessage"] = response.Value;
        //                    return RedirectToAction("Index");
        //                }

        //                TempData["InfoMessage"] = response.Error;
        //                return RedirectToAction("Index");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError($"Error occurred while updating blog post: {ex}");
        //            TempData["ErrorMessage"] = "An unexpected error occurred while updating the blog post. Please try again.";
        //            return RedirectToAction("Index");
        //        }
        //    }

        //    return View(dto);

        //}
    }
}
