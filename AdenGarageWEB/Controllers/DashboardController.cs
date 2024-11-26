using Microsoft.AspNetCore.Mvc;

namespace AdenGarageWEB.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
