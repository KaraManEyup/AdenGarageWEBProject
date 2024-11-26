using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;  // IEmailSender arayüzü
using Microsoft.AspNetCore.Mvc;
using MimeKit.Encodings;
using System.Text.Encodings.Web; // URL kodlaması için


public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly UrlEncoder _urlEncoder;
    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, UrlEncoder urlEncoder)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _urlEncoder = urlEncoder;
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                Address = model.Address,
                Gender = model.Gender
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // E-posta doğrulama token'ını oluştur
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedToken = _urlEncoder.Encode(token); // Token'ı URL'ye güvenli hale getir
                var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = encodedToken }, Request.Scheme);

                // E-posta gönder
                await _emailSender.SendEmailAsync(model.Email, "E-posta Doğrulama",
                    $"Hesabınızı doğrulamak için <a href='{confirmationLink}'>bu bağlantıyı</a> tıklayın.");

                TempData["EmailSent"] = "E-posta adresinize doğrulama linki gönderildi.";

                return RedirectToAction("RegisterConfirmation");
            }

            // Hata varsa
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

    public IActionResult RegisterConfirmation()
    {
        return View();  // Bu, RegisterConfirmation.cshtml view'ini döndürecektir
    }

    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (userId == null || token == null)
        {
            return RedirectToAction("Index", "Home");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return RedirectToAction("Index", "Home");
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
        {
            return RedirectToAction("ConfirmEmailSuccess");
        }

        return RedirectToAction("ConfirmEmailFailed");
    }

    public IActionResult ConfirmEmailSuccess()
    {
        return View(); // Başarılı e-posta doğrulama mesajı
    }

    public IActionResult ConfirmEmailFailed()
    {
        return View(); // Hatalı doğrulama mesajı
    }


    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError("", "Geçersiz giriş denemesi.");
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login");
        }



        var model = new ProfileViewModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            DateOfBirth = user.DateOfBirth,
            Address = user.Address,
            Gender = user.Gender
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Profile(ProfileViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Şu an giriş yapan kullanıcıyı alıyoruz
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return RedirectToAction("Login");
        }

        // Kullanıcı bilgilerini güncelliyoruz
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.DateOfBirth = model.DateOfBirth;
        user.Address = model.Address;
        user.Gender = model.Gender;
        user.Email = model.Email;

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            ViewBag.SuccessMessage = "Profiliniz başarıyla güncellendi.";
            return View(model);
        }

        // Güncelleme sırasında hata varsa, bunları gösteriyoruz
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    public async Task<IActionResult> EditProfile()
    {
        var userId = _userManager.GetUserId(User); 
        var user = await _userManager.FindByIdAsync(userId); 

        if (user == null)
        {
            return NotFound();
        }

        var model = new EditProfileViewModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            DateOfBirth = user.DateOfBirth,  
            Address = user.Address,
            Gender = user.Gender
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> EditProfile(EditProfileViewModel model)
    {

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login");
        }

        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Email = model.Email;
        user.DateOfBirth = model.DateOfBirth;
        user.Address = model.Address;
        user.Gender = model.Gender;

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            TempData["SuccessMessage"] = "Profiliniz başarıyla güncellendi.";
            return RedirectToAction("Profile");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
