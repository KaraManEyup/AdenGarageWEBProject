using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

public class BaseController : Controller
{
    protected readonly UserManager<ApplicationUser> _userManager;

    public BaseController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    protected async Task<ApplicationUser> GetCurrentUserAsync()
    {
        return await _userManager.GetUserAsync(User);
    }

    protected string GetCurrentUserId()
    {
        return _userManager.GetUserId(User);
    }
}
