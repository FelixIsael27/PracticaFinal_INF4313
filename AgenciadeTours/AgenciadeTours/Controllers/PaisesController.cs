using AgenciadeTours.Data;
using AgenciadeTours.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace AgenciadeTours.Controllers
{
    public class PaisesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaisesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Lista()
        {
            return View(await _context.Paises.ToListAsync());
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Pais pais)
        {
            if (ModelState.IsValid)
            {
                if (_context.Paises.Any(x => x.PaisID == pais.PaisID))
                {
                    ModelState.AddModelError("", "El ID ya existe.");
                    return View(pais);
                }

                try
                {
                    _context.Paises.Add(pais);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Lista));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error: " + ex.Message);
                }
            }
            return View(pais);
        }

        public async Task<IActionResult> Editar(int id)
        {
            var pais = await _context.Paises.FindAsync(id);
            if (pais == null) return NotFound();
            return View(pais);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Pais pais)
        {
            if (ModelState.IsValid)
            {
                var existe = await _context.Paises.AnyAsync(x => x.PaisID == pais.PaisID);
                if (!existe)
                {
                    ModelState.AddModelError("", "El ID no existe.");
                    return View(pais);
                }

                try
                {
                    _context.Paises.Update(pais);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Lista));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error: " + ex.Message);
                }
            }
            return View(pais);
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var pais = await _context.Paises.FindAsync(id);
            if (pais == null) return NotFound();
            return View(pais);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarEliminacion(int id)
        {
            var pais = await _context.Paises.FindAsync(id);
            if (pais == null)
            {
                ModelState.AddModelError("", "No existe el ID.");
                return RedirectToAction(nameof(Lista));
            }

            try
            {
                _context.Paises.Remove(pais);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
            }

            return RedirectToAction(nameof(Lista));
        }

        public async Task<IActionResult> ExportarCSV()
        {
            var lista = await _context.Paises.ToListAsync();
            var sb = new StringBuilder();

            sb.AppendLine("PaisID,Nombre");

            foreach (var p in lista)
                sb.AppendLine($"{p.PaisID},{p.Nombre}");

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "Paises.csv");
        }
    }
}
