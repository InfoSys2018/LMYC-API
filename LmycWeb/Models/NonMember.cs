using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace LmycWeb.Models
{
    public class NonMember
    {
        [Key]
        public string NonMemberId { get; set; }

        public string BookingId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public Booking Booking { get; set; }

        public string Name { get; set; }

    }
}
