using AgenciadeTours.Data;
using AgenciadeTours.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace AgenciadeTours.Controllers
{
    public class ToursController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ToursController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Mostrar()
        {
            var tours = await _context.Tours
                .Include(t => t.Pais)
                .Include(t => t.Destino)
                .ToListAsync();

            return View(tours);
        }

        public IActionResult Agregar()
        {
            ViewBag.Paises = _context.Paises.ToList();
            ViewBag.Destinos = _context.Destinos.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(Tour model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Paises = _context.Paises.ToList();
                ViewBag.Destinos = _context.Destinos.ToList();
                return View(model);
            }

            if (_context.Tours.Any(x => x.TourID == model.TourID))
            {
                ModelState.AddModelError("", "El ID del tour ya existe.");
                return View(model);
            }

            model.ITBIS = Math.Round(model.Precio * 0.18m, 2);

            var destino = await _context.Destinos.FindAsync(model.DestinoID);
            if (destino == null) 
            {
                ModelState.AddModelError("", "Destino no encontrado");
                return View(destino);
            }

            var duracion = new TimeSpan(destino.Dias_Duracion, destino.Horas_Duracion, 0);
            model.FechaHoraFin = model.Fecha.Add(duracion);

            model.Estado = model.FechaHoraInicio >= DateTime.Now ? "Vigente" : "No vigente";

            _context.Tours.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Mostrar));
        }

        public async Task<IActionResult> Actualizar(int id)
        {
            var tour = await _context.Tours.FindAsync(id);
            ViewBag.Paises = _context.Paises.ToList();
            ViewBag.Destinos = _context.Destinos.ToList();
            return View(tour);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Actualizaar(Tour model)
        {
            if (!_context.Tours.Any(x => x.TourID == model.TourID))
            {
                ModelState.AddModelError("", "El ID no existe.");
                return View(model);
            }

            model.ITBIS = Math.Round(model.Precio * 0.18m, 2);

            var destino = await _context.Destinos.FindAsync(model.DestinoID);
            if (destino == null)
            {
                ModelState.AddModelError("", "Destino no encontrado");
                return View(destino);
            }

            var duracion = new TimeSpan(destino.Dias_Duracion, destino.Horas_Duracion, 0);
            model.FechaHoraFin = model.Fecha.Add(duracion);

            model.Estado = model.FechaHoraInicio >= DateTime.Now ? "Vigente" : "No vigente";

            _context.Tours.Update(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Mostrar));
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var tour = await _context.Tours
                .Include(x => x.Destino)
                .Include(x => x.Pais)
                .FirstOrDefaultAsync(x => x.TourID == id);

            if (tour == null) return NotFound();
            return View(tour);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarEliminacion(int id)
        {
            var tour = await _context.Tours.FindAsync(id);
            if (tour == null) return NotFound();

            _context.Tours.Remove(tour);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Mostrar));
        }

        public async Task<IActionResult> ExportarCSV()
        {
            var lista = await _context.Tours
                .Include(a => a.Pais)
                .Include(a => a.Destino)
                .ToListAsync();

            var sb = new StringBuilder();
            sb.AppendLine("ID,Nombre,Pais,Destino,Fecha,Hora,Precio,ITBIS,Estado");

            foreach (var t in lista)
            {
                sb.AppendLine($"{t.TourID},{t.Nombre},{t.Pais.Nombre},{t.Destino.Nombre},{t.Fecha.ToShortDateString()},{t.Hora},{t.Precio},{t.ITBIS},{t.Estado}");
            }

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "Tours.csv");
        }
    }
}
