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

        public IActionResult Index()
        {
<<<<<<< HEAD
            ViewBag.Message = "Welcome to Organizer Dashboard!";
            Debug.WriteLine("OrganizerDashboard Index loaded");

            //  Add a sample EventRequest for testing if none exist
            if (!_context.EventRequests.Any())
            {
                var testRequest = new EventRequest
                {
                    CustomerId = _context.Users.FirstOrDefault()?.Id, // pick first user
                    SubCategoryId = _context.SubCategories.FirstOrDefault()?.Id ?? 1, // fallback to 1
                    Budget = 50000,
                    VenuePreference = "Test Venue",
                    GuestCapacity = 100,
                    EventDate = DateTime.Today.AddDays(7),
                    EventTime = new TimeSpan(18, 0, 0),
                    RequestedServices = "Decoration, Catering",
                    AdditionalNotes = "This is a test event request",
                    Status = "Pending",
                    IsOrganized = false
                };

                _context.EventRequests.Add(testRequest);
                _context.SaveChanges();

                Debug.WriteLine(" Test EventRequest inserted into database.");
            }

            return View();
=======
            // Show pending event requests to organizer
            var pending = _context.EventRequests
                                  .Where(r => r.Status == "Pending")
                                  .OrderByDescending(r => r.EventDate)
                                  .ToList();
            return View(pending);
>>>>>>> a88007fef8cb8e44953d08126adb205698512ccd
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

        public IActionResult PendingRequests()
        {
            var pendingRequests = _context.EventRequests
                .Include(r => r.Customer)
                .Where(r => r.IsOrganized == false)
                .ToList();

            return View(pendingRequests);
        }

        public IActionResult CreateTemplate(int id)
        {
            var eventRequest = _context.EventRequests.Find(id);
            if (eventRequest == null) return NotFound();

            ViewBag.RequestId = id;
            return View();
        }

        [HttpPost]
        public IActionResult CreateTemplate(int eventRequestId, decimal estimatedBudget)
        {
            var organizerId = _userManager.GetUserId(User);

            var template = new EventTemplate
            {
                EventRequestId = eventRequestId,
                OrganizerId = organizerId,
                EstimatedBudget = estimatedBudget,
                Status = "Draft"
            };

            _context.EventTemplates.Add(template);

            // Mark request as organized
            var request = _context.EventRequests.Find(eventRequestId);
            if (request != null)
            {
                request.IsOrganized = true;
            }

            _context.SaveChanges();

            // Redirect to vendor selection for this template
            return RedirectToAction("AddVendorsToTemplate", new { templateId = template.Id });
        }

        public IActionResult AddVendorsToTemplate(int templateId)
        {
            var template = _context.EventTemplates
                .Include(t => t.EventRequest)
                .FirstOrDefault(t => t.Id == templateId);

            if (template == null) return NotFound();

            var vendorServices = _context.VendorServices
                .Include(v => v.Vendor)
                .OrderByDescending(v => v.Rating)
                .ToList();

            ViewBag.Template = template;
            return View(vendorServices);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddVendorsToTemplate(int templateId, List<int> selectedVendorIds)
        {
            var template = _context.EventTemplates.Find(templateId);
            if (template == null) return NotFound();

            if (selectedVendorIds == null || !selectedVendorIds.Any())
            {
                ModelState.AddModelError("", "Please select at least one vendor.");
                var vendorServices = _context.VendorServices
                    .Include(v => v.Vendor)
                    .OrderByDescending(v => v.Rating)
                    .ToList();
                ViewBag.Template = template;
                return View(vendorServices);
            }

            foreach (var vendorId in selectedVendorIds)
            {
                var tv = new TemplateVendor
                {
                    EventTemplateId = templateId,
                    VendorServiceId = vendorId,
                    Status = "Pending"
                };
                _context.TemplateVendors.Add(tv);
            }

            _context.SaveChanges();

            return RedirectToAction("ReviewTemplate", new { templateId = templateId });
        }

        public IActionResult ReviewTemplate(int templateId)
        {
            var template = _context.EventTemplates
                .Include(t => t.TemplateVendors)
                .ThenInclude(tv => tv.VendorService)
                .ThenInclude(v => v.Vendor)
                .FirstOrDefault(t => t.Id == templateId);

            if (template == null) return NotFound();

            return View(template);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmTemplate(int templateId)
        {
            var template = _context.EventTemplates
                .Include(t => t.EventRequest)
                .Include(t => t.TemplateVendors)
                .ThenInclude(tv => tv.VendorService)
                .FirstOrDefault(t => t.Id == templateId);

            if (template == null)
            {
                TempData["Error"] = "Template not found.";
                return RedirectToAction("Index");
            }

            template.Status = "Confirmed";
            _context.EventTemplates.Update(template);

            TempData["Success"] = $"Template #{template.Id} confirmed and sent to customer.";

            _context.SaveChanges();

            // Redirect to dashboard
            return RedirectToAction("Index");
        }

    }
}
