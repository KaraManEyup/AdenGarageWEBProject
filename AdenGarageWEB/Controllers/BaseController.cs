using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

public class BaseController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public BaseController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (User.Identity.IsAuthenticated)
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (user != null)
            {
                ViewData["UserName"] = user.FullName; // Kullanıcının adını ViewData'ya ekle
            }
        }

        base.OnActionExecuting(context);
    }
}
