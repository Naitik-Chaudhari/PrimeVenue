using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrimeVenue.Model;
using PrimeVenue.Repository;

namespace PrimeVenue.Controllers
{
    public class EventRequestController : Controller
    {
        private readonly IEventRequestRepository _eventRequestRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventRequestController(IEventRequestRepository eventRequestRepo,
                                      UserManager<ApplicationUser> userManager)
        {
            _eventRequestRepo = eventRequestRepo;
            _userManager = userManager;
        }

        // ---------------- List All Requests (Organizer/Admin)
        public IActionResult Index()
        {
            var requests = _eventRequestRepo.GetAll();
            return View(requests);
        }

        // ---------------- View Requests for a Customer
        [Authorize(Roles = "Customer")]
        public IActionResult MyRequests(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
                return BadRequest("Customer ID is required");

            var requests = _eventRequestRepo.GetByCustomer(customerId);
            return View(requests);
        }

        // ---------------- Details of a Single Request
        public IActionResult Details(int id)
        {
            var request = _eventRequestRepo.GetById(id);
            if (request == null)
                return NotFound();

            return View(request);
        }

        // ---------------- Create Event Request (GET)
        [Authorize(Roles = "Customer")]
        public IActionResult Create(int subCategoryId)
        {
            // Pre-fill SubCategoryId
            if (subCategoryId <= 0) return BadRequest("Invalid subCategoryId");

            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId)) return Challenge();
            var request = new EventRequest
            {
                SubCategoryId = subCategoryId,
                CustomerId = userId
            };

            return View(request);
        }

        // ---------------- Create Event Request (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public IActionResult Create(EventRequest request)
        {
            // Ensure CustomerId comes from the logged-in user, not the form
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId)) return Challenge();
            request.CustomerId = userId;

            // Remove navigation properties from validation
            ModelState.Remove(nameof(EventRequest.Customer));
            ModelState.Remove(nameof(EventRequest.SubCategory));
            ModelState.Remove(nameof(EventRequest.Templates));

            // Ensure defaults
            request.Status = "Pending";
            request.IsOrganized = false;

            if (ModelState.IsValid)
            {
                _eventRequestRepo.Add(request);
                // After creation, show customer's list (status Pending by default)
                return RedirectToAction("Index", "CustomerDashboard");
            }
            return View(request);
        }

        // ---------------- Update Event Request (GET)
        public IActionResult Edit(int id)
        {
            var request = _eventRequestRepo.GetById(id);
            if (request == null)
                return NotFound();

            return View(request);
        }

        // ---------------- Update Event Request (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EventRequest request)
        {
            if (ModelState.IsValid)
            {
                _eventRequestRepo.Update(request);
                return RedirectToAction("Details", new { id = request.Id });
            }
            return View(request);
        }

        // ---------------- Delete Event Request
        public IActionResult Delete(int id)
        {
            _eventRequestRepo.Delete(id);
            return RedirectToAction("Index");
        }

        // ---------------- Mark Event as Organized & Add Rating
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CompleteEvent(int id, int rating)
        {
            var request = _eventRequestRepo.GetById(id);
            if (request == null)
                return NotFound();

            request.IsOrganized = true;
            request.Rating = rating;
            _eventRequestRepo.Update(request);

            return RedirectToAction("Details", new { id = id });
        }
    }
}
