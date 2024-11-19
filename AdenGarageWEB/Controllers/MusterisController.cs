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

            var musteriler = _context.Musteriler.AsQueryable();

            if (string.IsNullOrEmpty(sortOrder))
            {
                sortOrder = "IsimAsc";  // Default sıralama
            }

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

            var musterilerList = await musteriler.ToListAsync();

            return View(musterilerList);  
        }

        // GET: Musteris/Create
        public IActionResult Create()
        {
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Isim,Soyisim,Telefon")] Musteri musteri)
        {
            if (ModelState.IsValid)
            {
                _context.Add(musteri);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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

        // GET: Musteris/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound(); // ID gönderilmezse NotFound döner
            }

            var musteri = await _context.Musteriler
                .FirstOrDefaultAsync(m => m.Id == id); // Veritabanından ID'ye göre müşteri getir

            if (musteri == null)
            {
                return NotFound(); // Belirtilen ID'ye ait müşteri bulunamazsa NotFound döner
            }

            return View(musteri); // Müşteri detayını View'a gönderir
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

        private bool MusteriExists(int id)
        {
            return _context.Musteriler.Any(e => e.Id == id);
        }

    }
}