using AdenGarageWEB.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure Entity Framework and SQL Server
builder.Services.AddDbContext<AdenGarageDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AdenGarageDbContext")));


var app = builder.Build();
var cultureInfo = new CultureInfo("tr-TR");
cultureInfo.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Default route configuration
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Musteris}/{action=Index}/{id?}");

app.Run();
