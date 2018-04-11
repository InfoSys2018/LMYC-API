using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LmycWeb.Models
{
    public class ClassificationCode
    {
        [Key]
        public string CodeId { get; set; }
        public string Classification { get; set; }
    }
}
