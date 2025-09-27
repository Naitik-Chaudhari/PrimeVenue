using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimeVenue.Model
{
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        // Navigation (not required on form post)
        public Category? Category { get; set; }
    }
}
