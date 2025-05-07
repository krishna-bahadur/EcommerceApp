using EcommerceApp.Application.DTOs.Auth;
using EcommerceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            model.ReturnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await _authService.RegisterAsync(model);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Account Created Successfully.";
                    return Json(new { success = true, redirectUrl = model.ReturnUrl });
                }

                return Json(new { success = false, errors = new[] { result.Error } });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            model.ReturnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await _authService.LoginAsync(model);

                if (result.IsSuccess)
                {
                    // Redirect SuperAdmin to Dashboard
                    if (result.Value.Roles.Contains("SuperAdmin"))
                    {
                        model.ReturnUrl = Url.Action("Index", "dashboard");
                    }

                    return Json(new { success = true, redirectUrl = model.ReturnUrl });
                }

                return Json(new { success = false, errors = new[] { result.Error } });
            }

            return Json(new { success = false });
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
