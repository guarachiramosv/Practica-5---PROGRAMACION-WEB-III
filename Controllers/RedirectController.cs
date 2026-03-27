using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using practica5PR.Models;

namespace practica5web.Controllers
{
    [Authorize]
    public class RedirectController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RedirectController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Administrador"))
            {
                return RedirectToAction("Index", "Dashboard");
            }
            else if (User.IsInRole("Farmaceutico"))
            {
                return RedirectToAction("Index", "Medicamentos");
            }
            else
            {
                return RedirectToAction("Index", "Medicamentos");
            }
        }
    }
}
