using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrimeVenue.Model;

namespace PrimeVenue.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var user = _userManager.GetUserAsync(User).Result; // sync version
            var roles = _userManager.GetRolesAsync(user).Result;

            if (roles.Contains("Vendor"))
                return RedirectToAction("Index", "VendorDashboard");

            if (roles.Contains("Organizer"))
                return RedirectToAction("Index", "OrganizerDashboard");

            if (roles.Contains("Customer"))
                return RedirectToAction("Index", "CustomerDashboard");

            return RedirectToAction("Index", "Home");
        }
    }
}
