using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace LmycWeb.Models
{
    public class Member
    {
        [Key]
        public string BookingId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public Booking Booking { get; set; }

        public int AllocatedCredits { get; set; }

        [Key]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }


    }
}
