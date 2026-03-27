using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using practica5PR.Services.Interfaces;
using practica5PR.Models.ViewModels;

namespace practica5web.Controllers
{
    [Authorize(Roles = "Administrador,Farmaceutico")]
    public class ReportesController : Controller
    {
        private readonly IInventarioService _inventarioService;

        public ReportesController(IInventarioService inventarioService)
        {
            _inventarioService = inventarioService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Vencidos()
        {
            var inventario = await _inventarioService.ObtenerEstadoInventarioAsync();
            var model = new ReporteViewModel
            {
                Titulo = "Reporte de Medicamentos Vencidos",
                Medicamentos = inventario.Vencidos
            };
            return View(model);
        }

        public async Task<IActionResult> PorVencer()
        {
            var inventario = await _inventarioService.ObtenerEstadoInventarioAsync();
            var model = new ReporteViewModel
            {
                Titulo = "Reporte de Medicamentos Próximos a Vencer",
                Medicamentos = inventario.PorVencer30Dias
            };
            return View(model);
        }

        public async Task<IActionResult> BajoStock()
        {
            var inventario = await _inventarioService.ObtenerEstadoInventarioAsync();
            var model = new ReporteViewModel
            {
                Titulo = "Reporte de Medicamentos con Bajo Stock",
                Medicamentos = inventario.BajoStock
            };
            return View(model);
        }

        public async Task<IActionResult> InventarioCompleto()
        {
            var inventario = await _inventarioService.ObtenerEstadoInventarioAsync();
            return View(inventario);
        }
    }
}
