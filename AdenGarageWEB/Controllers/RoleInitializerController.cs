using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class RoleInitializerController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleInitializerController(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<IActionResult> InitializeRoles()
    {
        if (!await _roleManager.RoleExistsAsync("Admin"))
            await _roleManager.CreateAsync(new IdentityRole("Admin"));

        if (!await _roleManager.RoleExistsAsync("User"))
            await _roleManager.CreateAsync(new IdentityRole("User"));

        return Content("Roller oluşturuldu.");
    }


}
