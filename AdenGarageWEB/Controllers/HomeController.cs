using AdenGarageWEB.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AdenGarageWEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly AdenGarageDbContext _context;

        public HomeController(AdenGarageDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchTerm)
        {
            var musteriler = _context.Musteriler
                .Include(m => m.Arabalar) // Arabalar ilişkisini dahil edin
                .Where(m =>
                    string.IsNullOrEmpty(searchTerm) || // Arama terimi yoksa tüm kayıtları getir
                    m.Isim.Contains(searchTerm) ||
                    m.Soyisim.Contains(searchTerm) ||
                    m.Telefon.Contains(searchTerm) ||
                    m.Arabalar.Any(a =>
                        a.Marka.Contains(searchTerm) ||
                        a.Model.Contains(searchTerm) ||
                        a.Plaka.Contains(searchTerm)))
                .ToList(); // Veritabanından veriyi çekin

            return View(musteriler);
        }

    }
}
