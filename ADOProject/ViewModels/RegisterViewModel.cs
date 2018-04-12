using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ADOProject.ViewModels
{
    public class RegisterViewModel 
    {
        [Required(ErrorMessage = "A username is required")]
        [MaxLength(30)]
        [Remote("UsernameAvailable", "Register", ErrorMessage = "This username is already in use")]
        public string Username { get; set; }


        [Required(ErrorMessage = "An email address is required")]
        [EmailAddress]
        [Remote("EmailAvailable", "Register", ErrorMessage = "This email address is already in use")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A password is required")]
        [StringLength(255, MinimumLength = 7, ErrorMessage = "Password must be 7 characters or longer")]
        public string Password { get; set; }

        [Required(ErrorMessage = "A password is required")]
        [Display(Name = "Confirm Password")]
        [StringLength(255, MinimumLength = 7, ErrorMessage = "Password must be 7 characters or longer")]
        [System.ComponentModel.DataAnnotations.Compare(nameof(Password))]
        public string ValidatePassword { get; set; }

    }
}