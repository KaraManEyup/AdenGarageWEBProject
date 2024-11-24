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
            : base(userManager) // BaseController'dan UserManager al�n�yor
        {
            _context = context;
        }

        // Anasayfa
        public async Task<IActionResult> Index(string searchTerm)
        {
            // M��teri ve ili�kili arabalar� sorgula, arama kriterlerini uygula
            var musteriler = await _context.Musteriler
                .Include(m => m.Arabalar) // Arabalar ili�kisini dahil et
                .Where(m =>
                    string.IsNullOrEmpty(searchTerm) || // Arama terimi bo�sa t�m kay�tlar� getir
                    m.Isim.Contains(searchTerm) ||
                    m.Soyisim.Contains(searchTerm) ||
                    m.Telefon.Contains(searchTerm) ||
                    m.Arabalar.Any(a =>
                        a.Marka.Contains(searchTerm) ||
                        a.Model.Contains(searchTerm) ||
                        a.Plaka.Contains(searchTerm)))
                .ToListAsync(); // Asenkron veri �ekme

            return View(musteriler);
        }

        // Hakk�nda sayfas� (iste�e ba�l�)
        public IActionResult About()
        {
            return View();
        }

        // �leti�im sayfas� (iste�e ba�l�)
        public IActionResult Contact()
        {
            return View();
        }
    }
}
