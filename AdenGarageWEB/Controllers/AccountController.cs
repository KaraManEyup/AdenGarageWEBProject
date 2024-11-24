using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AdenGarageWEB.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace AdenGarageWEB.Controllers
{
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

            ViewData["Title"] = "Register";
            return View(); // Register view'ını döndür
        }
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User); // Giriş yapan kullanıcıyı alıyoruz
            if (user == null)
            {
                return RedirectToAction("Login", "Account"); // Kullanıcı bulunamazsa login sayfasına yönlendir
            }

            // Kullanıcı bilgilerini Profile view'ına gönderiyoruz
            return View(user);
        }



        // Register POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    Gender = model.Gender
                };

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

            return View(model); // Hata varsa formu tekrar döndür
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
                        // Giriş başarılı ise, ana sayfaya yönlendiriyoruz
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError(string.Empty, "Geçersiz giriş.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı.");
                }
            }

            return View(model); // Hata varsa formu tekrar gösteriyoruz
        }


        // EditProfile Action
        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new EditProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth
            };

            return View(model);
        }


        // Profil Düzenleme POST
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
                user.DateOfBirth = model.DateOfBirth;
                user.Gender = model.Gender;

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
            return View(model); // Hata varsa düzenleme formunu yeniden göster
        }

        // Kullanıcı Çıkışı
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home"); // Çıkış yaptıktan sonra anasayfaya yönlendiriyoruz
        }
    }
}
