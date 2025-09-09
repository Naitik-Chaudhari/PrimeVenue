using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimeVenue.Model
{
    public class VendorService
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string VendorId { get; set; }   // FK to ApplicationUser

        [Required]
        public string ServiceType { get; set; } // Catering, Venue, Decoration

        [Range(1000, 1000000)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PriceEstimate { get; set; }

        [Range(1, 5)]
        public double Rating { get; set; }

        // Navigation
        public ApplicationUser Vendor { get; set; }
    }
}
