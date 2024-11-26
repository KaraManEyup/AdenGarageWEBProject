using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
public class DashboardController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public DashboardController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var users = _userManager.Users.ToList();
        return View(users);
    }

    // Kullanıcıyı Düzenleme
    [HttpPost]
    public async Task<IActionResult> EditUserRole(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);
            await _userManager.AddToRoleAsync(user, role);
        }
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Dashboard()
    {
        var user = await _userManager.GetUserAsync(User); // Giriş yapan kullanıcıyı al
        if (user == null)
        {
            return RedirectToAction("Login");
        }

        // Kullanıcının rollerini almak için
        var roles = await _userManager.GetRolesAsync(user);

        // Rolleri ve kullanıcıyı modelde kullanmak
        var model = new DashboardViewModel
        {
            User = user,
            Roles = roles.ToList()  // Rolleri listeye dönüştür
        };

        return View(model);  // Modeli View'a gönder
    }
}
