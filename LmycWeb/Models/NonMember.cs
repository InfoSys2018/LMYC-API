using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LmycWeb.Models
{
    public class NonMember
    {
        [Key]
        public string NonMemberId { get; set; }

        public string BookingId { get; set; }
        public Booking Booking { get; set; }

        public string Name { get; set; }

    }
}
