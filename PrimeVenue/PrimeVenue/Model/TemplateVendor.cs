using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimeVenue.Model
{
    public class TemplateVendor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("EventTemplate")]
        public int EventTemplateId { get; set; }

        [Required]
        [ForeignKey("VendorService")]
        public int VendorServiceId { get; set; }

        [DefaultValue("Pending")]
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Declined

        // Navigation
        public EventTemplate EventTemplate { get; set; }
        public VendorService VendorService { get; set; }
    }
}
