using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LmycWeb.ViewModels
{
    public class ClubMembersViewModel {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Email { get; set; }
        [Display(Name = "Member Status")]
        public string MemberStatus { get; set; }
        [Display(Name = "Skipper Status")]
        public string SkipperStatus { get; set; }
        [Display(Name = "Mobile Phone")]
        public string MobilePhone { get; set; }
        public string Skills { get; set; }
        [Display(Name = "Sailing Experience")]
        public string SailingExperience { get; set; }
    }
}
