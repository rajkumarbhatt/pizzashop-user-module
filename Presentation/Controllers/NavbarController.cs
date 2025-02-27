using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentaion.Controllers
{
    [Authorize (Roles = "Super Admin, Account Manager")]
    public class NavbarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}