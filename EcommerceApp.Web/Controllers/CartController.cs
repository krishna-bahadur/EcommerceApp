using Azure;
using EcommerceApp.Application.DTOs.Cart;
using EcommerceApp.Application.DTOs.Shipping;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Domain.Entities;
using EcommerceApp.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommerceApp.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _cartService.GetOrCreateCartAsync(userId);
            if (result.IsSuccess)
            {
                return View(result.Value);
            }

            return View(new CartDto());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToCart(Guid id, int quantity)
        {
            
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _cartService.AddToCartAsync(userId, id, quantity);

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = result.Value;
                return RedirectToAction("index", "cart");
            }

            TempData["InfoMessage"] = result.Error;
            return RedirectToAction("/");
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(Guid id, int quantity)
        {
            ShippingPageDto shippingPageDto = new ShippingPageDto();

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _cartService.GetOrCreateCartAsync(userId);
            if (result.IsSuccess)
            {
                var productDto = result.Value;
                decimal? price = result.Value.TotalPrice;

                //Direct buy have only 1 item
                int totalItem = result.Value.CartItemDto.Count();

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
