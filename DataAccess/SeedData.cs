using AdenGarageWEB.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        var context = serviceProvider.GetRequiredService<AdenGarageDbContext>();

        // Veritabanı oluşturulmamışsa oluştur
        context.Database.EnsureCreated();

        // Admin rolü yoksa oluştur
        var roleExist = await roleManager.RoleExistsAsync("Admin");
        if (!roleExist)
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        // Admin kullanıcısı yoksa oluştur
        var adminUser = await userManager.FindByEmailAsync("eyupskaraman@gmail.com");
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = "admin@admin.com",
                Email = "eyupskaraman.com",
                FirstName = "Eyup Admin",
                LastName = "Karaman",
                DateOfBirth = new DateTime(1980, 1, 1),
                Address = "Admin Address",
                Gender = "Male"
            };

            var result = await userManager.CreateAsync(adminUser, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
