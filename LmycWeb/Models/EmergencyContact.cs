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
        public string Name1 { get; set; }
        public string Phone1 { get; set; }
        public string Name2 { get; set; }
        public string Phone2 { get; set; }
    }
}
