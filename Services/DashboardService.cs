using Microsoft.EntityFrameworkCore;
using practica5PR.Data;
using practica5PR.Models.ViewModels;
using practica5PR.Services.Interfaces;

namespace practica5PR.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardViewModel> ObtenerResumenAsync()
        {
            var hoy = DateTime.Today;
            var fechaLimite30 = hoy.AddDays(30);
            const int stockMinimo = 10;

            var totalMedicamentos = await _context.Medicamentos.CountAsync();

            var medicamentosVencidos = await _context.Medicamentos
                .CountAsync(m => m.FechaVencimiento < hoy);

            var medicamentosPorVencer = await _context.Medicamentos
                .CountAsync(m => m.FechaVencimiento >= hoy && m.FechaVencimiento <= fechaLimite30);

            var medicamentosBajoStock = await _context.Medicamentos
                .CountAsync(m => m.Stock <= stockMinimo);

            var totalCategorias = await _context.Categorias.CountAsync();
            var totalEstantes = await _context.Estantes.CountAsync();

            return new DashboardViewModel
            {
                TotalMedicamentos = totalMedicamentos,
                MedicamentosVencidos = medicamentosVencidos,
                MedicamentosPorVencer = medicamentosPorVencer,
                MedicamentosBajoStock = medicamentosBajoStock,
                TotalCategorias = totalCategorias,
                TotalEstantes = totalEstantes
            };
        }
    }
}