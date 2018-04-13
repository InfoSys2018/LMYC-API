using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LmycWeb.Models
{
    public class EmergencyContact
    {
        [Key]
        public string EmergencyContactId { get; set; }
        [Display(Name = "Emergency Contact #1 Name")]
        public string Name1 { get; set; }
        [Display(Name = "Emergency Contact #1 Phone Number")]
        [Phone]
        public string Phone1 { get; set; }
        [Display(Name = "Emergency Contact #2 Name")]
        public string Name2 { get; set; }
        [Display(Name = "Emergency Contact #2 Phone Number")]
        [Phone]
        public string Phone2 { get; set; }
    }
}
