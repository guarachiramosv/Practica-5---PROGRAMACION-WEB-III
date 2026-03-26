using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using practica5PR.Services.Interfaces;

namespace practica5web.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _dashboardService.ObtenerResumenAsync();
            return View(model);
        }
    }
}
