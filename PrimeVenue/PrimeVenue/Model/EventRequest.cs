using System.Collections.Generic;
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

        [Required]// FK to ApplicationUser
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

        [StringLength(500)]
        public string AdditionalNotes { get; set; }

        [DefaultValue("Pending")]
        public string Status { get; set; } = "Pending"; // Pending, TemplateSent, Confirmed, Cancelled

        // Navigation
        public ApplicationUser Customer { get; set; }
        public SubCategory SubCategory { get; set; }
        public ICollection<EventTemplate> Templates { get; set; }
    }
}
