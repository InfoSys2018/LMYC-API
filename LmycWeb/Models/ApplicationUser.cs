using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace LmycWeb.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Member Status")]
        public string MemberStatus { get; set; }
        [Display(Name = "Skipper Status")]
        public string SkipperStatus { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        public string Country { get; set; }
        [Display(Name = "Mobile Phone")]
        public string MobilePhone { get; set; }
        [Display(Name = "Home Phone")]
        public string HomePhone { get; set; }
        [Display(Name = "Work Phone")]
        public string WorkPhone { get; set; }
        [Display(Name = "Sailing Qualifications")]
        public string SailingQualifications { get; set; }
        public string Skills { get; set; }
        [Display(Name = "Sailing Experience")]
        public string SailingExperience { get; set; }
        public int Credits { get; set; }
        public string EmergencyContactId { get; set; }
        public EmergencyContact EmergencyContacts { get; set; }
        public List<Booking> Bookings { get; set; }
        public List<Report> Reports { get; set; }
    }
}
