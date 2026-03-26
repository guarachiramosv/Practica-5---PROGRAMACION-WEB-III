using Microsoft.EntityFrameworkCore;
using practica5PR.Data;
using practica5PR.Models.ViewModels;
using practica5PR.Services.Interfaces;

namespace practica5PR.Services
{
    public class InventarioService : IInventarioService
    {
        private readonly ApplicationDbContext _context;

        public InventarioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<InventarioViewModel> ObtenerEstadoInventarioAsync()
        {
            var hoy = DateTime.Today;
            var fecha30 = hoy.AddDays(30);
            var fecha60 = hoy.AddDays(60);
            var fecha90 = hoy.AddDays(90);
            const int stockMinimo = 10;

            var baseQuery = _context.Medicamentos
                .Include(m => m.Categoria)
                .Include(m => m.Estante)
                .AsQueryable();

            var vencidos = await baseQuery
                .Where(m => m.FechaVencimiento < hoy)
                .OrderBy(m => m.FechaVencimiento)
                .ToListAsync();

            var porVencer30 = await baseQuery
                .Where(m => m.FechaVencimiento >= hoy && m.FechaVencimiento <= fecha30)
                .OrderBy(m => m.FechaVencimiento)
                .ToListAsync();

            var porVencer60 = await baseQuery
                .Where(m => m.FechaVencimiento > fecha30 && m.FechaVencimiento <= fecha60)
                .OrderBy(m => m.FechaVencimiento)
                .ToListAsync();

            var porVencer90 = await baseQuery
                .Where(m => m.FechaVencimiento > fecha60 && m.FechaVencimiento <= fecha90)
                .OrderBy(m => m.FechaVencimiento)
                .ToListAsync();

            var bajoStock = await baseQuery
                .Where(m => m.Stock <= stockMinimo)
                .OrderBy(m => m.Stock)
                .ThenBy(m => m.Nombre)
                .ToListAsync();

            return new InventarioViewModel
            {
                Vencidos = vencidos,
                PorVencer30Dias = porVencer30,
                PorVencer60Dias = porVencer60,
                PorVencer90Dias = porVencer90,
                BajoStock = bajoStock
            };
        }
    }
}