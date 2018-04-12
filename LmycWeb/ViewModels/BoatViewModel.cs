using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LmycWeb.ViewModels
{
    public class BoatViewModel
    {
        public string BoatId { get; set; }
        public string Name { get; set; }
        [Display(Name = "Credits Per Hour")]
        public int CreditsPerHour { get; set; }
        public string Status { get; set; }
        public IFormFile Photo { get; set; }

        public string Description { get; set; }
        public int Length { get; set; }
        public string Make { get; set; }
        public int Year { get; set; }
    }
}
