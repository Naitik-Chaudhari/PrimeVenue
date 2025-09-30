using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimeVenue.Model
{
    public class EventTemplate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("EventRequest")]
        public int EventRequestId { get; set; }

        [Required]
        public string OrganizerId { get; set; }

        [Range(1000, 1000000)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal EstimatedBudget { get; set; }

        [DefaultValue("Draft")]
        public string Status { get; set; } = "Draft"; // Draft, Confirmed, SentToCustomer, Finalized

        // Navigation
        public EventRequest EventRequest { get; set; }
        public ApplicationUser Organizer { get; set; }
        public ICollection<TemplateVendor> TemplateVendors { get; set; }
    }
}
