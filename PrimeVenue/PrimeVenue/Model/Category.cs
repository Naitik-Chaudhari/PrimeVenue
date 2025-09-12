using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PrimeVenue.Model
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } // Social Event, Corporate Event etc.

        public string ImageUrl { get; set; }

        // Navigation
        public ICollection<SubCategory> SubCategories { get; set; }
    }
}
