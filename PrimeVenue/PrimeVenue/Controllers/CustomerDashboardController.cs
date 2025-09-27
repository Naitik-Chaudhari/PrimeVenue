using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrimeVenue.Model;
using PrimeVenue.Repository;

namespace PrimeVenue.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerDashboardController : Controller
    {
        private readonly IEventRequestRepository _eventRequestRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerDashboardController(IEventRequestRepository eventRequestRepo,
                                           UserManager<ApplicationUser> userManager)
        {
            _eventRequestRepo = eventRequestRepo;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId)) return Challenge();
            var requests = _eventRequestRepo.GetByCustomer(userId);
            return View(requests);
        }
    }
}
