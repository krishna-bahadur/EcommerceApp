using EcommerceApp.Domain.Constants.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Web.Controllers
{
    public class DashboardController : Controller
    {
        [Authorize(Roles = UserRoles.SuperAdmin)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
