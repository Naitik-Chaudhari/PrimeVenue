using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PrimeVenue.Controllers
{
    [Authorize(Roles = "Vendor")]
    public class VendorDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
