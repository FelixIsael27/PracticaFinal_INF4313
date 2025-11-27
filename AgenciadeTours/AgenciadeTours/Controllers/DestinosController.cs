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
        public async Task<IActionResult> Agregar(Destino model)
        {
            if (ModelState.IsValid)
            {
                if (_context.Destinos.Any(x => x.DestinoID == model.DestinoID))
                {
                    ModelState.AddModelError("", "El ID ya existe.");
                    ViewBag.Paises = _context.Paises.ToList();
                    return View(model);
                }

                _context.Destinos.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Mostrar));
            }
            ViewBag.Paises = _context.Paises.ToList();
            return View(model);
        }

        public async Task<IActionResult> Actualizar(int id)
        {
            var destino = await _context.Destinos.FindAsync(id);
            ViewBag.Paises = _context.Paises.ToList();
            return View(destino);
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar(Destino model)
        {
            if (!_context.Destinos.Any(x => x.DestinoID == model.DestinoID))
            {
                ModelState.AddModelError("", "El ID no existe.");
                ViewBag.Paises = _context.Paises.ToList();
                return View(model);
            }

            _context.Destinos.Update(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Mostrar));
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var destino = await _context.Destinos.Include(a => a.Pais).FirstOrDefaultAsync(a => a.DestinoID == id);
            return View(destino);
        }

        [HttpPost, ActionName("Eliminar")]
        public async Task<IActionResult> ConfirmarEliminacion(int id)
        {
            var destino = await _context.Destinos.FindAsync(id);
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
