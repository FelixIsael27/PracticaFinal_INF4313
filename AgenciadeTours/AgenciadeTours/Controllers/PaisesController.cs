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

        public async Task<IActionResult> Mostrar()
        {
            return View(await _context.Paises.ToListAsync());
        }

        public IActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(Pais model)
        {
            if (ModelState.IsValid)
            {
                if (_context.Paises.Any(x => x.PaisID == model.PaisID))
                {
                    ModelState.AddModelError("", "El ID ya existe.");
                    return View(model);
                }

                try
                {
                    _context.Paises.Add(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Mostrar));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error: " + ex.Message);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Actualizar(int id)
        {
            var pais = await _context.Paises.FindAsync(id);
            if (pais == null) return NotFound();
            return View(pais);
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar(Pais model)
        {
            if (ModelState.IsValid)
            {
                var existe = await _context.Paises.AnyAsync(x => x.PaisID == model.PaisID);
                if (!existe)
                {
                    ModelState.AddModelError("", "El ID no existe.");
                    return View(model);
                }

                try
                {
                    _context.Paises.Update(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Mostrar));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error: " + ex.Message);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var pais = await _context.Paises.FindAsync(id);
            if (pais == null) return NotFound();
            return View(pais);
        }

        [HttpPost, ActionName("Eliminar")]
        public async Task<IActionResult> ConfirmarEliminacion(int id)
        {
            var pais = await _context.Paises.FindAsync(id);
            if (pais == null)
            {
                ModelState.AddModelError("", "No existe el ID.");
                return RedirectToAction(nameof(Mostrar));
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

            return RedirectToAction(nameof(Mostrar));
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
