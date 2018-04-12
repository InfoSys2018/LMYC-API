using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LmycWeb.Models
{
    public class Boat
    {
        [Key]
        public string BoatId { get; set; }
        [Display(Name = "Boat Name")]
        public string Name { get; set; }
        public int CreditsPerHour { get; set; }
        public string Status { get; set; }
        public byte[] Photo { get; set; }
        public string Description { get; set; }
        public int Length { get; set; }
        public string Make { get; set; }
        public int Year { get; set; }

        public List<Booking> Bookings { get; set; }
    }
}
