using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using practica5PR.Data;
using practica5PR.Models;
using Microsoft.AspNetCore.Hosting;

namespace practica5web.Controllers
{
    [Authorize]
    public class MedicamentosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public MedicamentosController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Medicamentos
        public async Task<IActionResult> Index(string searchString, int? categoriaId, int? estanteId)
        {
            var applicationDbContext = _context.Medicamentos
                .Include(m => m.Categoria)
                .Include(m => m.Estante)
                .AsQueryable();

            // Filtrado por nombre
            if (!string.IsNullOrEmpty(searchString))
            {
                applicationDbContext = applicationDbContext.Where(s => s.Nombre.Contains(searchString));
            }

            // Filtrado por categoría
            if (categoriaId.HasValue)
            {
                applicationDbContext = applicationDbContext.Where(m => m.CategoriaId == categoriaId);
            }

            // Filtrado por estante
            if (estanteId.HasValue)
            {
                applicationDbContext = applicationDbContext.Where(m => m.EstanteId == estanteId);
            }

            ViewData["Categorias"] = new SelectList(_context.Categorias, "Id", "Nombre", categoriaId);
            ViewData["Estantes"] = new SelectList(_context.Estantes, "Id", "Nombre", estanteId);
            ViewData["CurrentFilter"] = searchString;

            var results = await applicationDbContext.ToListAsync();

            if (User.IsInRole("Cliente"))
            {
                return View("IndexCliente", results);
            }

            return View("IndexAdmin", results);
        }

        // GET: Medicamentos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var medicamento = await _context.Medicamentos
                .Include(m => m.Categoria)
                .Include(m => m.Estante)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicamento == null) return NotFound();

            return View(medicamento);
        }

        // GET: Medicamentos/Create
        [Authorize(Roles = "Administrador,Farmaceutico")]
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre");
            ViewData["EstanteId"] = new SelectList(_context.Estantes, "Id", "Nombre");
            return View();
        }

        // POST: Medicamentos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Farmaceutico")]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Precio,Stock,FechaVencimiento,CategoriaId,EstanteId,Descripcion,Estado")] Medicamento medicamento, IFormFile? imagen)
        {
            if (ModelState.IsValid)
            {
                if (imagen != null && imagen.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "medicamentos");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imagen.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imagen.CopyToAsync(fileStream);
                    }
                    
                    medicamento.ImagenUrl = "/images/medicamentos/" + uniqueFileName;
                }

                _context.Add(medicamento);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Medicamento registrado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", medicamento.CategoriaId);
            ViewData["EstanteId"] = new SelectList(_context.Estantes, "Id", "Nombre", medicamento.EstanteId);
            return View(medicamento);
        }

        // GET: Medicamentos/Edit/5
        [Authorize(Roles = "Administrador,Farmaceutico")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var medicamento = await _context.Medicamentos.FindAsync(id);
            if (medicamento == null) return NotFound();

            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", medicamento.CategoriaId);
            ViewData["EstanteId"] = new SelectList(_context.Estantes, "Id", "Nombre", medicamento.EstanteId);
            return View(medicamento);
        }

        // POST: Medicamentos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Farmaceutico")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Precio,Stock,FechaVencimiento,CategoriaId,EstanteId,Descripcion,Estado,ImagenUrl")] Medicamento medicamento, IFormFile? imagen)
        {
            if (id != medicamento.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (imagen != null && imagen.Length > 0)
                    {
                        // Guardar la nueva imagen
                        string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "medicamentos");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imagen.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imagen.CopyToAsync(fileStream);
                        }

                        // Eliminar la imagen anterior si existe
                        if (!string.IsNullOrEmpty(medicamento.ImagenUrl))
                        {
                            string oldPath = Path.Combine(_hostEnvironment.WebRootPath, medicamento.ImagenUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldPath))
                            {
                                System.IO.File.Delete(oldPath);
                            }
                        }

                        medicamento.ImagenUrl = "/images/medicamentos/" + uniqueFileName;
                    }

                    _context.Update(medicamento);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Medicamento actualizado correctamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicamentoExists(medicamento.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", medicamento.CategoriaId);
            ViewData["EstanteId"] = new SelectList(_context.Estantes, "Id", "Nombre", medicamento.EstanteId);
            return View(medicamento);
        }

        // GET: Medicamentos/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var medicamento = await _context.Medicamentos
                .Include(m => m.Categoria)
                .Include(m => m.Estante)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicamento == null) return NotFound();

            return View(medicamento);
        }

        // POST: Medicamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicamento = await _context.Medicamentos.FindAsync(id);
            if (medicamento != null)
            {
                // Eliminar imagen del servidor
                if (!string.IsNullOrEmpty(medicamento.ImagenUrl))
                {
                    string filePath = Path.Combine(_hostEnvironment.WebRootPath, medicamento.ImagenUrl.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _context.Medicamentos.Remove(medicamento);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Medicamento eliminado correctamente.";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool MedicamentoExists(int id)
        {
            return _context.Medicamentos.Any(e => e.Id == id);
        }
    }
}
