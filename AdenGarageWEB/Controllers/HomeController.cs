using AdenGarageWEB.DataAccess;
using AdenGarageWEB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AdenGarageWEB.Controllers
{
    public class HomeController : BaseController
    {
        private readonly AdenGarageDbContext _context;

        public HomeController(AdenGarageDbContext context, UserManager<ApplicationUser> userManager)
            : base(userManager) // BaseController'dan UserManager alýnýyor
        {
            _context = context;
        }

        // Anasayfa
        public async Task<IActionResult> Index(string searchTerm)
        {
            // Müþteri ve iliþkili arabalarý sorgula, arama kriterlerini uygula
            var musteriler = await _context.Musteriler
                .Include(m => m.Arabalar) // Arabalar iliþkisini dahil et
                .Where(m =>
                    string.IsNullOrEmpty(searchTerm) || // Arama terimi boþsa tüm kayýtlarý getir
                    m.Isim.Contains(searchTerm) ||
                    m.Soyisim.Contains(searchTerm) ||
                    m.Telefon.Contains(searchTerm) ||
                    m.Arabalar.Any(a =>
                        a.Marka.Contains(searchTerm) ||
                        a.Model.Contains(searchTerm) ||
                        a.Plaka.Contains(searchTerm)))
                .ToListAsync(); // Asenkron veri çekme

            return View(musteriler);
        }

        // Hakkýnda sayfasý (isteðe baðlý)
        public IActionResult About()
        {
            return View();
        }

        // Ýletiþim sayfasý (isteðe baðlý)
        public IActionResult Contact()
        {
            return View();
        }
    }
}
