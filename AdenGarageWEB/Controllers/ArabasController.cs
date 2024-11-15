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
    public class ArabasController : Controller
    {
        private readonly AdenGarageDbContext _context;

        public ArabasController(AdenGarageDbContext context)
        {
            _context = context;
        }

        // GET: Arabas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Arabalar.ToListAsync());
        }

        // GET: Arabas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var araba = await _context.Arabalar
                .FirstOrDefaultAsync(m => m.Id == id);
            if (araba == null)
            {
                return NotFound();
            }

            return View(araba);
        }

        // GET: Arabas/Create
        public IActionResult Create()
        {
            ViewBag.MusteriId = new SelectList(
                _context.Musteriler
                    .Select(m => new { m.Id, FullName = m.Isim + " " + m.Soyisim })
                    .ToList(),
                "Id",
                "FullName"
            );

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Marka,Model,Km,Islem,Tarih,MusteriId")] Araba araba)
        {
            if (ModelState.IsValid)
            {
                _context.Add(araba);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


            return View(araba);
        }

        // GET: Arabas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var araba = await _context.Arabalar.FindAsync(id);
            if (araba == null)
            {
                return NotFound();
            }
            return View(araba);

            ViewBag.MusteriId = new SelectList(_context.Musteriler, "Id", "Ad");

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Marka,Model,Km,Islem,Tarih,MusteriId")] Araba araba)
        {
            if (id != araba.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(araba);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArabaExists(araba.Id))
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
            return View(araba);
        }

        // GET: Arabas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var araba = await _context.Arabalar
                .FirstOrDefaultAsync(m => m.Id == id);
            if (araba == null)
            {
                return NotFound();
            }

            return View(araba);
        }

        // POST: Arabas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var araba = await _context.Arabalar.FindAsync(id);
            if (araba != null)
            {
                _context.Arabalar.Remove(araba);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArabaExists(int id)
        {
            return _context.Arabalar.Any(e => e.Id == id);
        }
    }
}
