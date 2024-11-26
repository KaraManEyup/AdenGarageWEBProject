using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Encodings;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly UrlEncoder _urlEncoder;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        UrlEncoder urlEncoder,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _urlEncoder = urlEncoder;
        _roleManager = roleManager;
    }

    // GET: Register Page
    [HttpGet]
    public IActionResult Register() => View();

    // POST: Register Page
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
                // Kullanıcıya rol ataması
                var roleResult = await _userManager.AddToRoleAsync(user, "User");  // Varsayılan 'User' rolü

                // E-posta doğrulama token'ını oluştur
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedToken = _urlEncoder.Encode(token);
                var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = encodedToken }, Request.Scheme);

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

    [HttpPost]
    public async Task<IActionResult> SeedData()
    {
        // Admin rolünü oluştur
        var roleExist = await _roleManager.RoleExistsAsync("Admin");
        if (!roleExist)
        {
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        // Admin kullanıcısını kontrol et
        var user = await _userManager.FindByEmailAsync("eyupskaraman@gmail.com");

        if (user == null)
        {
            // Admin kullanıcısını oluştur
            user = new ApplicationUser
            {
                UserName = "eyupskaraman@gmail.com",
                Email = "eyupskaraman@gmail.com",
                FirstName = "EyupADmin",
                LastName = "Admin",
                EmailConfirmed = true,
                DateOfBirth = new DateTime(1990, 1, 1),
                Address = "Admin Address",
                Gender = "Male"
            };

            var result = await _userManager.CreateAsync(user, "Admin1234!"); // Admin şifresi

            if (result.Succeeded)
            {
                // Admin rolünü ekle
                await _userManager.AddToRoleAsync(user, "Admin");
            }
        }

        // Başarı mesajı döndür
        TempData["SeedSuccess"] = "Admin hesabı ve rol başarıyla oluşturuldu!";
        return RedirectToAction("Index", "Home");  // Ana sayfaya yönlendir
    }


    // GET: Login Page
    [HttpGet]
    public IActionResult Login() => View();

    // POST: Login Page
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

    // GET: Profile View
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

    // POST: Update Profile
    [HttpPost]
    public async Task<IActionResult> Profile(ProfileViewModel model)
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

        // Update user profile details
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

        // If update fails, show errors
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    // GET: Edit Profile Page
    [HttpGet]
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

    // POST: Update Profile from EditPage
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

        // If update fails, show errors
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    // Confirm Email Action
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

    // Registration Confirmation View
    public IActionResult RegisterConfirmation()
    {
        return View();
    }

    // Email Confirmation Success View
    public IActionResult ConfirmEmailSuccess()
    {
        return View();
    }

    // Email Confirmation Failed View
    public IActionResult ConfirmEmailFailed()
    {
        return View();
    }

    // Logout Action
    [HttpPost]
    [ValidateAntiForgeryToken] // Güvenlik için CSRF koruması ekleyin
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
