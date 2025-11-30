using AgenciadeTours.Data;
using AgenciadeTours.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace AgenciadeTours.Controllers
{
    public class DestinosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DestinosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Mostrar()
        {
            return View(await _context.Destinos.Include(x => x.Pais).ToListAsync());
        }

        public IActionResult Agregar()
        {
            ViewBag.Paises = _context.Paises.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(Destino destino)
        {
            if (ModelState.IsValid)
            {
                if (_context.Destinos.Any(x => x.DestinoID == destino.DestinoID))
                {
                    ModelState.AddModelError("", "El ID del Destino ya existe.");
                    ViewBag.Paises = _context.Paises.ToList();
                    return View(destino);
                }

                _context.Destinos.Add(destino);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Mostrar));
            }
            ViewBag.Paises = _context.Paises.ToList();
            return View(destino);
        }

        public async Task<IActionResult> Actualizar(int id)
        {
            var destino = await _context.Destinos.FindAsync(id);
            if (destino == null) return NotFound();

            ViewBag.Paises = _context.Paises.ToList();
            return View(destino);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Actualizar(Destino destino)
        {
            if (!_context.Destinos.Any(x => x.DestinoID == destino.DestinoID))
            {
                ModelState.AddModelError("", "El ID no existe.");
                ViewBag.Paises = _context.Paises.ToList();
                return View(destino);
            }

            _context.Destinos.Update(destino);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Mostrar));
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var destino = await _context.Destinos.Include(a => a.Pais).FirstOrDefaultAsync(a => a.DestinoID == id);
            if (destino == null) return NotFound();
            return View(destino);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarEliminacion(int id)
        {
            var destino = await _context.Destinos.FindAsync(id);
            if (destino == null) return NotFound();

            _context.Destinos.Remove(destino);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Mostrar));
        }

        public async Task<IActionResult> ExportarCSV()
        {
            var lista = await _context.Destinos.Include(x => x.Pais).ToListAsync();
            var sb = new StringBuilder();

            sb.AppendLine("DestinoID,Nombre,Pais");

            foreach (var d in lista)
                sb.AppendLine($"{d.DestinoID},{d.Nombre},{d.Pais.Nombre}");

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "Destinos.csv");
        }
    }
}
