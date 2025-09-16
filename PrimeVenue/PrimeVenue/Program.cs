using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrimeVenue.Model;
using PrimeVenue.Repository;

var builder = WebApplication.CreateBuilder(args);

//  1. Configure Database Connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

//  3. Register Repositories
builder.Services.AddScoped<IEventRequestRepository, EventRequestRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IVendorServiceRepository, VendorServiceRepository>();
builder.Services.AddScoped<IEventTemplateRepository, EventTemplateRepository>();
builder.Services.AddScoped<ITemplateVendorRepository, TemplateVendorRepository>();

//  4. Add MVC with Razor Runtime Compilation
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await IdentitySeeder.SeedRolesAsync(services);
    await IdentitySeeder.SeedAdminAsync(services);
}

//  6. Configure Middleware
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // Must come BEFORE Authorization
app.UseAuthorization();

//  7. Configure Routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
