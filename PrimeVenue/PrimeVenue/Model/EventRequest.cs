using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimeVenue.Model
{
    public class EventRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CustomerId { get; set; }

        [Required]
        public int SubCategoryId { get; set; }

        [Required(ErrorMessage = "Budget is Required")]
        [Range(100, 1000000, ErrorMessage = "Budget must be between 100 and 1,000,000")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Budget { get; set; }
        public string VenuePreference { get; set; }

        [Range(1, 5000)]
        public int GuestCapacity { get; set; }

        [Required(ErrorMessage = "Event Date is Required"), DataType(DataType.Date)]
        public DateTime EventDate { get; set; }

        [Required(ErrorMessage = "Event Time is Required"), DataType(DataType.Time)]
        public TimeSpan EventTime { get; set; }

        [StringLength(300)]
        public string RequestedServices { get; set; }

        [StringLength(500)]
        public string AdditionalNotes { get; set; }

        [DefaultValue("Pending")]
        public string Status { get; set; } = "Pending"; // Pending, TemplateSent, Confirmed, Cancelled

        [DefaultValue(false)]
        public bool IsOrganized { get; set; } = false;

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int? Rating { get; set; }

<<<<<<< HEAD
        // Navigation (ignored in form validation)
        [ValidateNever]
=======
        public int? FinalizedTemplateId { get; set; }  // null until user finalizes one
        public EventTemplate FinalizedTemplate { get; set; }

        // Navigation
>>>>>>> 3fe11a56f7cac732bca4d82cdd0b97b7f331a557
        public ApplicationUser Customer { get; set; }
        [ValidateNever]
        public SubCategory SubCategory { get; set; }
        [ValidateNever]
        public ICollection<EventTemplate> Templates { get; set; }
    }
}
