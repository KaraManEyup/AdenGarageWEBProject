using AdenGarageWEB.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure Entity Framework and SQL Server
builder.Services.AddDbContext<AdenGarageDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AdenGarageDbContext")));

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;
})
    .AddEntityFrameworkStores<AdenGarageDbContext>()
    .AddDefaultTokenProviders();

// Localization (CultureInfo)
var cultureInfo = new CultureInfo("tr-TR");
cultureInfo.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Add session or any other necessary services here
// builder.Services.AddSession();

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add authentication middleware
app.UseAuthentication(); // Important to add authentication middleware
app.UseAuthorization();

// Default route configuration
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Musteris}/{action=Index}/{id?}");

app.Run();
