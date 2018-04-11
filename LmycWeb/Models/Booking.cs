using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LmycWeb.Models
{
    public class Booking
    {
        [Key]
        public string BookingId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int CreditsUsed { get; set; }

        public string BoatId { get; set; }
        public Boat Boat { get; set; }
        
        public string Itinerary { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public List<Member> Members { get; set; }
        public List<NonMember> NonMembers { get; set; }
    }
}
