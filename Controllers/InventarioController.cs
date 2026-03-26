using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using practica5PR.Services.Interfaces;

namespace practica5web.Controllers
{
    [Authorize(Roles = "Administrador,Farmaceutico")]
    public class InventarioController : Controller
    {
        private readonly IInventarioService _inventarioService;

        public InventarioController(IInventarioService inventarioService)
        {
            _inventarioService = inventarioService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _inventarioService.ObtenerEstadoInventarioAsync();
            return View(model);
        }
    }
}
