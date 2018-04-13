using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LmycWeb.ViewModels.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [StringLength(100, MinimumLength = 1)]
        [Required(ErrorMessage = "First Name field is required.")]
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(100, MinimumLength = 1)]
        [Required(ErrorMessage = "Last Name field is required.")]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [StringLength(100, MinimumLength = 1, ErrorMessage = "Street field must be between 1 & 100 characters")]
        [Required(ErrorMessage = "Street field is required.")]
        [DataType(DataType.Text)]
        public string Street { get; set; }

        [StringLength(100, MinimumLength = 1, ErrorMessage = "City field must be between 1 & 100 characters")]
        [Required(ErrorMessage = "City field is required.")]
        [DataType(DataType.Text)]
        public string City { get; set; }

        [Required(ErrorMessage = "Province field is required.")]
        [DataType(DataType.Text)]
        [Display(Name = "Province")]
        public string Province { get; set; }

        [Required(ErrorMessage = "Postal code field is required.")]
        [DataType(DataType.Text)]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [StringLength(100, MinimumLength = 1, ErrorMessage = "Country must be between 1 & 100 characters")]
        [Required(ErrorMessage = "Country field is required.")]
        [DataType(DataType.Text)]
        public string Country { get; set; }

        [Display(Name = "Home Phone")]
        [Phone]
        public string HomePhone { get; set; }

        [Required]
        [Display(Name = "Mobile Phone")]
        [Phone]
        public string MobilePhone { get; set; }

        [Display(Name = "Work Phone")]
        [Phone]
        public string WorkPhone { get; set; }

        [Required(ErrorMessage = "Your sailing qualifications is needed.")]
        [Display(Name = "Sailing Qualifications")]
        [DataType(DataType.Text)]
        public string SailingQualifications { get; set; }

        [Required(ErrorMessage = "Your sailing qualifications is needed.")]
        [DataType(DataType.Text)]
        public string Skills { get; set; }

        [Required(ErrorMessage = "Your sailing experience is needed.")]
        [Display(Name = "Sailing Experience")]
        public string SailingExperience { get; set; }
        
        [Display(Name = "Emergency Contact Name")]
        public string EmergencyContactName1 { get; set; }

        [Display(Name = "Emergency Contact Phone Number")]
        public string EmergencyContactPhone1 { get; set; }

        [Display(Name = "Emergency Contact Name")]
        public string EmergencyContactName2 { get; set; }

        [Display(Name = "Emergency Contact Phone Number")]
        public string EmergencyContactPhone2 { get; set; }

    }
}
