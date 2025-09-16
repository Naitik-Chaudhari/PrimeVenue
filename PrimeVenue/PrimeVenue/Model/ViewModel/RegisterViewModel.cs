﻿using System.ComponentModel.DataAnnotations;

namespace PrimeVenue.Model.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Role { get; set; } = "Customer";
    }
}
