using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AdenGarageWEB.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    // Register GET
    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity.IsAuthenticated) // Eğer kullanıcı zaten giriş yaptıysa, başka bir sayfaya yönlendir
        {
            return RedirectToAction("Index", "Musteris");
        }

        // Sayfa başlığını ViewData'ya ayarla
        ViewData["Title"] = "Register";

        return View(); // Register view'ını döndür
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FullName = model.FullName };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Musteris");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }


    // Login GET
    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated) // Eğer kullanıcı zaten giriş yaptıysa, başka bir sayfaya yönlendir
        {
            return RedirectToAction("Index", "Musteris");
        }
        return View();
    }

    // Login POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    // Giriş başarılı ise ana sayfaya yönlendiriyoruz
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Geçersiz giriş.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı.");
            }
        }

        // Hata varsa formu tekrar gösteriyoruz
        return View(model);
    }

    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User); // Giriş yapan kullanıcıyı alıyoruz
        if (user == null)
        {
            return RedirectToAction("Login", "Account"); // Eğer kullanıcı yoksa giriş sayfasına yönlendiriyoruz
        }
        return View(user); // Kullanıcıyı Profile view'ına gönderiyoruz
    }


    // Profil düzenleme sayfası
    [Authorize]
    public async Task<IActionResult> EditProfile()
    {
        var user = await _userManager.GetUserAsync(User); // Giriş yapan kullanıcıyı alıyoruz
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }
        var model = new EditProfileViewModel
        {
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };
        return View(model); // Kullanıcıyı düzenleme formuna gönderiyoruz
    }

    // Profil düzenleme işlemi
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProfile(EditProfileViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User); // Giriş yapan kullanıcıyı alıyoruz
            if (user == null)
            {
                return View(model); // Eğer kullanıcı bulunamadıysa, düzenleme formunu tekrar göster
            }

            // Kullanıcı bilgilerini güncelliyoruz
            user.FullName = model.FullName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            // Bilgileri kaydediyoruz
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                // Profil güncelleme başarılı ise, profil sayfasına yönlendiriyoruz
                return RedirectToAction("Profile");
            }
            else
            {
                // Profil güncellenirken bir hata oluştuysa, hata mesajını göster
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }
        // Hata varsa düzenleme formunu yeniden göster
        return View(model);
    }

    // Kullanıcı çıkışı
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();  // Kullanıcıyı çıkart
        return RedirectToAction("Index", "Home");  // Anasayfaya yönlendir
    }
}
