using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrimeVenue.Model;
using PrimeVenue.Repository;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{

})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<IEventRequestRepository, EventRequestRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IVendorServiceRepository, VendorServiceRepository>();
builder.Services.AddScoped<IEventTemplateRepository, EventTemplateRepository>();
builder.Services.AddScoped<ITemplateVendorRepository, TemplateVendorRepository>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.Run();
