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
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Credits Per Hour")]
        public int CreditsPerHour { get; set; }
        [Required]
        public string Status { get; set; }
        public IFormFile Photo { get; set; }
        public string Description { get; set; }
        [Required]
        public int Length { get; set; }
        [Required]
        public string Make { get; set; }
        [Required]
        public int Year { get; set; }
    }
}
