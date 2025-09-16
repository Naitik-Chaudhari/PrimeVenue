using Microsoft.AspNetCore.Mvc;
using PrimeVenue.Model;
using PrimeVenue.Repository;

namespace PrimeVenue.Controllers
{
    public class EventRequestController : Controller
    {
        private readonly IEventRequestRepository _eventRequestRepo;

        public EventRequestController(IEventRequestRepository eventRequestRepo)
        {
            _eventRequestRepo = eventRequestRepo;
        }

        // ---------------- List All Requests (Organizer/Admin)
        public IActionResult Index()
        {
            var requests = _eventRequestRepo.GetAll();
            return View(requests);
        }

        // ---------------- View Requests for a Customer
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
        public IActionResult Create(int subCategoryId)
        {
            // Pre-fill SubCategoryId
            var request = new EventRequest
            {
                SubCategoryId = subCategoryId,
                CustomerId = User.Identity.Name // or from your ApplicationUser Id
            };

            return View(request);
        }

        // ---------------- Create Event Request (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EventRequest request)
        {
            if (ModelState.IsValid)
            {
                _eventRequestRepo.Add(request);
                return RedirectToAction("MyRequests", new { customerId = request.CustomerId });
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
