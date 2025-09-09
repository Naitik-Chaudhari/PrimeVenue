using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrimeVenue.Model;

var builder = WebApplication.CreateBuilder(args);

// Add connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add DbContext + Identity
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // configure identity options if you want
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

var app = builder.Build();
app.MapGet("/", () => "Hello World!");
app.Run();