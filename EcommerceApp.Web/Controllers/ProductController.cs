using Azure;
using EcommerceApp.Application.DTOs.Product;
using EcommerceApp.Application.DTOs.Shipping;
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
            var result = await _productService.GetAllProductsAsync(10);
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

        [HttpGet]
        public async Task<IActionResult> Category(Guid id)
        {
            var result = await _productService.GetAllProductsAsync(10, id);
            if (result.IsSuccess)
            {
                return View(result.Value);
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
                        return RedirectToAction("Create");
                    }

                    TempData["InfoMessage"] = response.Error;
                    return View();
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
            var response = await _productService.GetProductForEditByIdAsync(id);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateProductDto dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _productService.UpdateProductAsync(dto);
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
                    TempData["ErrorMessage"] = "An unexpected error occurred while updating the product. Please try again.";
                    return RedirectToAction("Index");
                }
            }

            return View(dto);
        }
        [HttpGet("product/details/{slug}")]
        public async Task<IActionResult> Details(string slug)
        {
            var result = await _productService.GetProductBySlugAsync(slug);
            if (result.IsSuccess)
            {
                return View(result.Value);
            }

            TempData["InfoMessage"] = result.Error;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Shipping(Guid id, int quantity)
        {
            ShippingPageDto shippingPageDto = new ShippingPageDto();

            var result = await _productService.GetProductForEditByIdAsync(id);
            if (result.IsSuccess)
            {
                var productDto = result.Value;
                decimal? price;
                if (productDto.DiscountPrice != null && productDto.DiscountPrice.HasValue)
                {
                    price = productDto.DiscountPrice;
                }
                else
                {
                    price = productDto.Price;
                }
                // Multiply by total quantity
                price = price * quantity;

                //Direct buy have only 1 item
                int totalItem = 1;

                // Static Delivery fee
                decimal? deliveryFee = 75;

                // Delivery and total fee are added at last
                decimal? totalPrice = deliveryFee + price;

                shippingPageDto.OrderSummeryDto.TotalItems = totalItem;
                shippingPageDto.OrderSummeryDto.Price = (decimal) price;
                shippingPageDto.OrderSummeryDto.DeliveryFee =  (decimal) deliveryFee;
                shippingPageDto.OrderSummeryDto.TotalPrice = (decimal) totalPrice;

                return View(shippingPageDto);

            }

            TempData["InfoMessage"] = result.Error;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(Guid id, int quantity)
        {
            ShippingPageDto shippingPageDto = new ShippingPageDto();

            var result = await _productService.GetProductForEditByIdAsync(id);
            if (result.IsSuccess)
            {
                var productDto = result.Value;
                decimal? price;
                if (productDto.DiscountPrice != null && productDto.DiscountPrice.HasValue)
                {
                    price = productDto.DiscountPrice;
                }
                else
                {
                    price = productDto.Price;
                }
                // Multiply by total quantity
                price = price * quantity;

                //Direct buy have only 1 item
                int totalItem = 1;

                // Static Delivery fee
                decimal? deliveryFee = 75;

                // Delivery and total fee are added at last
                decimal? totalPrice = deliveryFee + price;

                shippingPageDto.OrderSummeryDto.TotalItems = totalItem;
                shippingPageDto.OrderSummeryDto.Price = (decimal) price;
                shippingPageDto.OrderSummeryDto.DeliveryFee = (decimal) deliveryFee;
                shippingPageDto.OrderSummeryDto.TotalPrice = (decimal) totalPrice;

                return View(shippingPageDto);

            }

            TempData["InfoMessage"] = result.Error;
            return View();
        }
    }
}
