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

        // GET: Musteris
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
            return View(musterilerList);
        }


        public IActionResult Create()
        {
            return View(new Musteri()); // Arabalar boş olarak dönebilir
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Musteri musteri)
        {
            if (musteri.Arabalar != null)
            {
                foreach (var araba in musteri.Arabalar)
                {
                    if (araba.Tarih == default)
                    {
                        araba.Tarih = DateTime.Now;  // Tarih boşsa, bugünün tarihi atanır
                    }
                }
            }

            if (ModelState.IsValid)
            {
                // Arabaları filtrele (Boş olanları kaldır)
                musteri.Arabalar = musteri.Arabalar?.Where(a =>
        !string.IsNullOrWhiteSpace(a.Marka) ||
        !string.IsNullOrWhiteSpace(a.Model) ||
        !string.IsNullOrWhiteSpace(a.Plaka) ||
        a.Tarih != default).ToList();

                // Müşteriyi ve arabalarını veritabanına ekle
                _context.Musteriler.Add(musteri);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(musteri);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musteri = await _context.Musteriler
                .Include(m => m.Arabalar) // Arabalar da yüklensin
                .FirstOrDefaultAsync(m => m.Id == id);

            if (musteri == null)
            {
                return NotFound();
            }

            return View(musteri);
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


        // GET: Musteris/Delete/5
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

        // POST: Musteris/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var musteri = await _context.Musteriler.FindAsync(id);
            if (musteri != null)
            {
                _context.Musteriler.Remove(musteri);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //private bool MusteriExists(int id)
        //{
        //    return _context.Musteriler.Any(e => e.Id == id);
        //}

    }
}