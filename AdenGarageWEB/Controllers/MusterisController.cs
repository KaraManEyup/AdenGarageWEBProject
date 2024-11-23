using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdenGarageWEB.DataAccess;
using Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdenGarageWEB.Controllers
{
    public class MusterisController : Controller
    {
        private readonly AdenGarageDbContext _context;

        public MusterisController(AdenGarageDbContext context)
        {
            _context = context;
        }

        // GET: Musteris
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["SortOrder"] = sortOrder ?? "IsimAsc";

            var musteriler = _context.Musteriler
                .Include(m => m.Arabalar)
                .AsQueryable();

            musteriler = sortOrder switch
            {
                "IsimDesc" => musteriler.OrderByDescending(m => m.Isim),
                _ => musteriler.OrderBy(m => m.Isim),
            };

            return View(await musteriler.ToListAsync());
        }

        // GET: Musteris/Create
        public IActionResult Create()
        {
            return View(new Musteri { Arabalar = new List<Araba>() });
        }

        // POST: Musteris/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Musteri musteri)
        {
            if (musteri.Arabalar != null)
            {
                // Boş araba bilgilerini temizle
                musteri.Arabalar = musteri.Arabalar
                    .Where(a => !string.IsNullOrWhiteSpace(a.Marka) ||
                                !string.IsNullOrWhiteSpace(a.Model) ||
                                !string.IsNullOrWhiteSpace(a.Plaka) ||
                                a.Tarih != default)
                    .ToList();
            }

            if (ModelState.IsValid)
            {
                _context.Musteriler.Add(musteri);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(musteri);
        }

        // GET: Musteris/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var musteri = await _context.Musteriler
                .Include(m => m.Arabalar)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (musteri == null)
                return NotFound();

            return View(musteri);
        }

        // GET: Musteris/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var musteri = await _context.Musteriler
                .Include(m => m.Arabalar)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (musteri == null)
                return NotFound();

            return View(musteri);
        }

        // POST: Musteris/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Musteri musteri)
        {
            if (id != musteri.Id)
                return NotFound();

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
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(musteri);
        }

        public async Task<IActionResult> EditAraba(int id)
        {
            var araba = await _context.Arabalar.FindAsync(id);
            if (araba == null)
            {
                return NotFound();
            }

            ViewData["MusteriId"] = new SelectList(_context.Musteriler, "Id", "Isim", araba.MusteriId);
            return View(araba);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAraba(int id, Araba araba)
        {
            if (id != araba.Id)
            {
                return NotFound();
            }

            var existingAraba = await _context.Arabalar
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            if (existingAraba == null)
            {
                return NotFound();
            }

            _context.Arabalar.Update(araba);  // Bu, nesneyi güncelleyecektir

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArabaExists(araba.Id))  // Burada ArabaExists metodunu kullanıyoruz
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            ViewData["MusteriId"] = new SelectList(_context.Musteriler, "Id", "Isim", araba.MusteriId);
            // İşlem tamamlandıktan sonra, arabayı içeren müşteri detaylarına yönlendirme
            return RedirectToAction("Details", new { id = araba.MusteriId });
        }
        private bool ArabaExists(int id)
        {
            return _context.Arabalar.Any(a => a.Id == id);
        }


        // GET: Musteris/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var musteri = await _context.Musteriler
                .Include(m => m.Arabalar)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (musteri == null)
                return NotFound();

            return View(musteri);
        }

        // POST: Musteris/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var musteri = await _context.Musteriler
                .Include(m => m.Arabalar)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (musteri != null)
            {
                _context.Arabalar.RemoveRange(musteri.Arabalar);
                _context.Musteriler.Remove(musteri);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MusteriExists(int id)
        {
            return _context.Musteriler.Any(e => e.Id == id);
        }
    }
}
