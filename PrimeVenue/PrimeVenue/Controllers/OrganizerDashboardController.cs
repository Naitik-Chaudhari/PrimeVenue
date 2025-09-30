using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeVenue.Model;
using PrimeVenue.Model.ViewModel;
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
        private readonly IEventRequestRepository _eventRequestRepo;
        private readonly IEventTemplateRepository _eventTemplateRepo;
        private readonly IVendorServiceRepository _vendorServiceRepo;
        private readonly ITemplateVendorRepository _templateVendorRepo;
        public OrganizerDashboardController(ApplicationDbContext context,
                                            UserManager<ApplicationUser> userManager,
                                            IServiceProvider serviceProvider,
                                            IEventRequestRepository eventRequestRepo,
                                            IEventTemplateRepository eventTemplateRepo,
                                            IVendorServiceRepository vendorServiceRepo,
                                            ITemplateVendorRepository templateVendorRepo)
        {
            _context = context;
            _userManager = userManager;
            _serviceProvider = serviceProvider;
            _eventRequestRepo = eventRequestRepo;
            _eventTemplateRepo = eventTemplateRepo;
            _vendorServiceRepo = vendorServiceRepo;
            _templateVendorRepo = templateVendorRepo;
        }

        public IActionResult Index()
        {
            var model = new OrganizerDashboardViewModel
            {
                PendingRequests = _eventRequestRepo.GetAll()
                                   .Where(r => r.Status == "Pending")
                                   .ToList(),

                TemplateSentRequests = _eventRequestRepo.GetAll()
                                       .Where(r => r.Status == "TemplateSent")
                                       .ToList(),

                FinalizedTemplateRequests = _eventRequestRepo.GetAll()
                                               .Where(r => r.Status == "FinalizedTemplate")
                                               .ToList()
            };

            return View(model);
        }

        // Show templates for a given request
        public IActionResult ViewTemplates(int eventRequestId)
        {
            var templates = _context.EventTemplates
                .Where(t => t.EventRequestId == eventRequestId)
                .ToList();

            ViewBag.EventRequestId = eventRequestId;
            return View(templates);
        }


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
            var pendingRequests = _eventRequestRepo.GetAll()
                                        .Where(r => r.Status == "Pending")
                                        .ToList();

            return View(pendingRequests);
        }

        public IActionResult CreateTemplate(int id)
        {
            var eventRequest = _eventRequestRepo.GetById(id);
            if (eventRequest == null) return NotFound();

            ViewBag.RequestId = id;
            return View();
        }

        [HttpPost]
        public IActionResult ConfirmCreateTemplate(int eventRequestId)
        {
            var organizerId = _userManager.GetUserId(User);

            var template = new EventTemplate
            {
                EventRequestId = eventRequestId,
                OrganizerId = organizerId,
                Status = "Draft"
            };

            _eventTemplateRepo.Add(template);

            return RedirectToAction("AddVendorsToTemplate", new { templateId = template.Id });
        }

        public IActionResult AddVendorsToTemplate(int templateId)
        {
            var template = _eventTemplateRepo.GetAll()
                .FirstOrDefault(t => t.Id == templateId);

            if (template == null) return NotFound();

            var vendorServices = _vendorServiceRepo.GetAll()
                .OrderByDescending(v => v.Rating)
                .ToList();

            ViewBag.Template = template;
            return View(vendorServices);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddVendorsToTemplate(int templateId, List<int> selectedVendorIds)
        {
            var template = _eventTemplateRepo.GetById(templateId);
            if (template == null) return NotFound();

            if (selectedVendorIds == null || !selectedVendorIds.Any())
            {
                ModelState.AddModelError("", "Please select at least one vendor.");
                var vendorServices = _vendorServiceRepo.GetAll()
                    .OrderByDescending(v => v.Rating)
                    .ToList();
                ViewBag.Template = template;
                return View(vendorServices);
            }

            var selectedVendors = _vendorServiceRepo.GetAll()
                .Where(v => selectedVendorIds.Contains(v.Id))
                .ToList();

            decimal totalBudget = selectedVendors.Sum(v => v.PriceEstimate);

            foreach (var vendor in selectedVendors)
            {
                var tv = new TemplateVendor
                {
                    EventTemplateId = templateId,
                    VendorServiceId = vendor.Id,
                    Status = "Pending"
                };
                _eventTemplateRepo.AddVendorToTemplate(tv);
            }

            template.EstimatedBudget = totalBudget;

            _context.SaveChanges();

            return RedirectToAction("ReviewTemplate", new { templateId = templateId });
        }

        public IActionResult EditVendorsForTemplate(int templateId)
        {
            var template = _eventTemplateRepo.GetAll()
                .FirstOrDefault(t => t.Id == templateId);

            if (template == null) return NotFound();

            var selectedVendorIds = _eventTemplateRepo.GetById(templateId)
                .TemplateVendors
                .Select(tv => tv.VendorServiceId)
                .ToList();

            var vendorServices = _vendorServiceRepo.GetAll()
                .OrderByDescending(v => v.Rating)
                .ToList();

            ViewBag.Template = template;
            ViewBag.SelectedVendorIds = selectedVendorIds;

            return View(vendorServices);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditVendorsForTemplate(int templateId, List<int> selectedVendorIds)
        {
            var template = _eventTemplateRepo.GetById(templateId);
            if (template == null) return NotFound();

            var existingVendorIds = _templateVendorRepo.GetByTemplate(templateId)
                .Select(tv => tv.VendorServiceId)
                .ToList();

            var vendorsToAdd = selectedVendorIds.Except(existingVendorIds).ToList();
            foreach (var vId in vendorsToAdd)
            {
                var TemplateVendor = new TemplateVendor
                {
                    EventTemplateId = templateId,
                    VendorServiceId = vId,
                    Status = "Pending"
                };

                _eventTemplateRepo.AddVendorToTemplate(TemplateVendor);
            }

            var vendorsToRemove = existingVendorIds.Except(selectedVendorIds).ToList();
            var toRemoveEntities = _context.TemplateVendors
                .Where(tv => tv.EventTemplateId == templateId && vendorsToRemove.Contains(tv.VendorServiceId));
            _context.TemplateVendors.RemoveRange(toRemoveEntities);

            var selectedVendors = _vendorServiceRepo.GetAll()
                .Where(v => selectedVendorIds.Contains(v.Id))
                .ToList();

            template.EstimatedBudget = selectedVendors.Sum(v => v.PriceEstimate);

            _context.SaveChanges();

            return RedirectToAction("ReviewTemplate", new { templateId = templateId });
        }



        public IActionResult ReviewTemplate(int templateId)
        {
            var template = _eventTemplateRepo.GetById(templateId);

            if (template == null) return NotFound();

            return View(template);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveTemplate(int templateId, int eventRequestId)
        {
            var template = _eventTemplateRepo.GetById(templateId);
            if (template == null) return NotFound();

            var templateVendors = _context.TemplateVendors
                .Where(tv => tv.EventTemplateId == templateId)
                .ToList();

            _context.TemplateVendors.RemoveRange(templateVendors);

            // Remove template
            _eventTemplateRepo.Delete(templateId);

            TempData["SuccessMessage"] = "Template removed successfully!";
            return RedirectToAction("ViewTemplates", new { eventRequestId = eventRequestId });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmTemplate(int templateId)
        {
            var template = _eventTemplateRepo.GetById(templateId);

            if (template == null)
            {
                TempData["Error"] = "Template not found.";
                return RedirectToAction("Index");
            }

            template.Status = "Confirmed";
            _eventTemplateRepo.Update(template);

            TempData["Success"] = $"Template #{template.Id} confirmed and sent to customer.";

            // Redirect to dashboard
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendTemplatesToUser(int eventRequestId)
        {
            var eventRequest = _eventRequestRepo.GetById(eventRequestId);
            if (eventRequest == null) return NotFound();

            // Update request status
            eventRequest.Status = "TemplateSent";

            // Update all templates for this request
            var templates = _eventTemplateRepo.GetAll()
                .Where(t => t.EventRequestId == eventRequestId)
                .ToList();

            foreach (var template in templates)
            {
                template.Status = "SentToCustomer";
            }

            _context.SaveChanges();

            TempData["SuccessMessage"] = "Templates have been sent to the customer!";
            return RedirectToAction("Index");
        }


    }
}
