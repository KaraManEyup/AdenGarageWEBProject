using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdenGarageWEB.DataAccess;
using Core.Models;

namespace AdenGarageWEB.Web.Controllers
{
    public class MusterisController : Controller
    {
        private readonly AdenGarageDbContext _context;

        public MusterisController(AdenGarageDbContext context)
        {
            _context = context;
        }

        // GET: Musteris
        public async Task<IActionResult> Index()
        {
            return View(await _context.Musteriler.ToListAsync());
        }

        // GET: Musteris/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Isim,Soyisim,Telefon")] Musteri musteri)
        {
            if (_context.Musteriler.Any(m =>
                m.Isim == musteri.Isim &&
                m.Soyisim == musteri.Soyisim &&
                m.Telefon == musteri.Telefon))
            {
                ModelState.AddModelError("", "Bu müşteri zaten kayıtlı.");
                return View(musteri);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(musteri);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Bir hata oluştu: " + ex.Message);
                }
            }

            return View(musteri);
        }

        // GET: Musteris/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musteri = await _context.Musteriler.FindAsync(id);
            if (musteri == null)
            {
                return NotFound();
            }
            return View(musteri);
        }

        // POST: Musteris/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Isim,Soyisim,Telefon")] Musteri musteri)
        {
            if (id != musteri.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(musteri);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MusteriExists(musteri.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(musteri);
        }

        // GET: Musteris/Delete/1
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musteri = await _context.Musteriler
                .FirstOrDefaultAsync(m => m.Id == id);

            if (musteri == null)
            {
                return NotFound();
            }

            return View(musteri);
        }

        // POST: Musteris/DeleteConfirmed/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var musteri = await _context.Musteriler.FindAsync(id);
            if (musteri == null)
            {
                return NotFound();
            }

            _context.Musteriler.Remove(musteri);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        private bool MusteriExists(int id)
        {
            return _context.Musteriler.Any(e => e.Id == id);
        }
    }
}
