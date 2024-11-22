using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdenGarageWEB.DataAccess;
using Core.Models;

namespace AdenGarageWEB.Controllers
{
    public class MusterisController : Controller
    {
        private readonly AdenGarageDbContext _context;

        public MusterisController(AdenGarageDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder)
        {
            // Varsayılan sıralama
            if (string.IsNullOrEmpty(sortOrder))
            {
                sortOrder = "IsimAsc"; // Default sıralama
            }

            // Müşteri verilerini arabalarla birlikte getiriyoruz
            var musteriler = _context.Musteriler
                .Include(m => m.Arabalar) // Arabaları dahil et
                .AsQueryable();

            // Sıralama işlemleri
            switch (sortOrder)
            {
                case "IsimDesc":
                    musteriler = musteriler.OrderByDescending(m => m.Isim);
                    break;
                case "IsimAsc":
                default:
                    musteriler = musteriler.OrderBy(m => m.Isim);
                    break;
            }

            // Listeyi döndürüyoruz
            var musterilerList = await musteriler.ToListAsync();
            return View(musterilerList); // Burada modelin bir koleksiyon olduğundan emin olun
        }

        public IActionResult Create()
        {
            return View(new Musteri()); // Arabalar boş olarak dönebilir
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Musteri musteri)
        {

            // Eğer arabalar yoksa veya tamamen boşsa, listeyi temizle
            if (musteri.Arabalar == null || !musteri.Arabalar.Any(a =>
                !string.IsNullOrWhiteSpace(a.Marka) ||
                !string.IsNullOrWhiteSpace(a.Model) ||
                !string.IsNullOrWhiteSpace(a.Plaka) ||
                a.Tarih != default))
            {
                musteri.Arabalar = new List<Araba>(); // Arabalar boş bırakılabilir
            }

            // ModelState doğrulamasını kontrol et
            if (ModelState.IsValid)
            {

                _context.Musteriler.Add(musteri);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // ModelState hataları varsa logla
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                           .Select(e => e.ErrorMessage);
            foreach (var error in errors)
            {
                Console.WriteLine(error);
            }

            // Model hatalıysa tekrar formu döndür
            return View(musteri);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musteri = await _context.Musteriler
                .Include(m => m.Arabalar)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (musteri == null)
            {
                return NotFound();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Musteri musteri)
        {
            if (id != musteri.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Arabalar ilişkilendirilmişse güncelle
                    foreach (var araba in musteri.Arabalar)
                    {
                        if (araba.Id == 0) // Yeni araba eklenmişse
                        {
                            _context.Arabalar.Add(araba);
                        }
                        else // Mevcut araba güncelleniyorsa
                        {
                            _context.Arabalar.Update(araba);
                        }
                    }

                    // Müşteriyi güncelle
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



        private bool MusteriExists(int id)
        {
            return _context.Musteriler.Any(e => e.Id == id);
        }

        // GET: Musteris/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musteri = await _context.Musteriler
                .Include(m => m.Arabalar)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (musteri == null)
            {
                return NotFound();
            }

            return View(musteri);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musteri = _context.Musteriler
                .Include(m => m.Arabalar) // Arabaları da dahil ediyoruz
                .FirstOrDefault(m => m.Id == id);

            if (musteri == null)
            {
                return NotFound();
            }

            return View(musteri);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var musteri = _context.Musteriler
                .Include(m => m.Arabalar)
                .FirstOrDefault(m => m.Id == id);

            if (musteri != null)
            {
                // İlgili arabaları da silmek isterseniz
                if (musteri.Arabalar != null && musteri.Arabalar.Any())
                {
                    _context.Arabalar.RemoveRange(musteri.Arabalar);
                }

                _context.Musteriler.Remove(musteri);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

    }
}