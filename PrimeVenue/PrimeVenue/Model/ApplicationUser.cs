using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PrimeVenue.Model
{
    public class ApplicationUser : IdentityUser
    {
        // Extra profile fields
        [Required]
        public string FullName { get; set; }


        [DefaultValue("Customer")]
        public string Role { get; set; } = "Customer"; // Organizer, Vendor, Customer

        // Navigation properties
        public ICollection<EventRequest> EventRequests { get; set; }  // For Customers
        public ICollection<VendorService> Services { get; set; }      // For Vendors
    }
}
