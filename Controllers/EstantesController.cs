using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practica5PR.Data;
using practica5PR.Models;

namespace practica5web.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class EstantesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EstantesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Estantes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Estantes.ToListAsync());
        }

        // GET: Estantes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var estante = await _context.Estantes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estante == null) return NotFound();

            return View(estante);
        }

        // GET: Estantes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Estantes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Ubicacion,Descripcion")] Estante estante)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estante);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estante);
        }

        // GET: Estantes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var estante = await _context.Estantes.FindAsync(id);
            if (estante == null) return NotFound();
            
            return View(estante);
        }

        // POST: Estantes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Ubicacion,Descripcion")] Estante estante)
        {
            if (id != estante.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstanteExists(estante.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(estante);
        }

        // GET: Estantes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var estante = await _context.Estantes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estante == null) return NotFound();

            return View(estante);
        }

        // POST: Estantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estante = await _context.Estantes.FindAsync(id);
            if (estante != null)
            {
                _context.Estantes.Remove(estante);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EstanteExists(int id)
        {
            return _context.Estantes.Any(e => e.Id == id);
        }
    }
}
