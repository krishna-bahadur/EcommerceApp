using EcommerceApp.Application.DTOs.Product;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EcommerceApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;

        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _productService.GetAllBlogsAsync(10);
            if(result.IsSuccess)
            {
                return View(result.Value);

            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
