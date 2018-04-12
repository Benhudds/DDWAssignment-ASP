using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ADOProject.Models;

namespace ADOProject.ViewModels
{
    public class EditUserViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string CreatedAt { get; set; }

        [Required]
        public int Level { get; set; }

        [Required(ErrorMessage = "A password is required")]
        [StringLength(255, MinimumLength = 7, ErrorMessage = "Password must be 7 characters or longer")]
        public string Password { get; set; }

        [Required(ErrorMessage = "A confirmed password is required")]
        [Display(Name = "Confirm Password")]
        [StringLength(255, MinimumLength = 7, ErrorMessage = "Password must be 7 characters or longer")]
        [System.ComponentModel.DataAnnotations.Compare(nameof(Password))]
        public string ValidatePassword { get; set; }
    }
}