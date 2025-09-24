using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeVenue.Model;
using PrimeVenue.Repository;
using System.Diagnostics;

namespace PrimeVenue.Controllers
{
    [Authorize(Roles = "Organizer")]
    public class OrganizerDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IServiceProvider _serviceProvider;

        public OrganizerDashboardController(ApplicationDbContext context,
                                            UserManager<ApplicationUser> userManager,
                                            IServiceProvider serviceProvider)
        {
            _context = context;
            _userManager = userManager;
            _serviceProvider = serviceProvider;
        }

        // Organizer Dashboard home
        public IActionResult Index()
        {
            ViewBag.Message = "Welcome to Organizer Dashboard!";
            Debug.WriteLine("OrganizerDashboard Index loaded");
            return View();
        }

        // GET: AddVendor form
        [HttpGet]
        public async Task<IActionResult> AddVendor()
        {
            // Get all users from DB
            var users = await _userManager.Users.ToListAsync();

            // Filter users who are NOT in Vendor role
            var nonVendorUsers = new List<ApplicationUser>();
            foreach (var user in users)
            {
                if (!await _userManager.IsInRoleAsync(user, "Vendor"))
                {
                    nonVendorUsers.Add(user);
                }
            }

            ViewBag.Users = nonVendorUsers;
            return View();
        }


        // POST: AddVendor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVendor(string vendorId, string serviceType, decimal priceEstimate)
        {
            if (string.IsNullOrEmpty(vendorId) || string.IsNullOrEmpty(serviceType))
            {
                ModelState.AddModelError("", "Please select a user and enter service type.");
                return await AddVendor(); // reload form
            }

            // Create VendorService
            var vendorService = new VendorService
            {
                VendorId = vendorId,
                ServiceType = serviceType,
                PriceEstimate = priceEstimate
            };

            _context.VendorServices.Add(vendorService);
            await _context.SaveChangesAsync();

            // Change user role to Vendor
            await IdentitySeeder.AssignVendorRoleAsync(_serviceProvider, vendorId);

            TempData["Success"] = "Vendor service added and role updated successfully!";
            return RedirectToAction("Index");
        }
    }
}
