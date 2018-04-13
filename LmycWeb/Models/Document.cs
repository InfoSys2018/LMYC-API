using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LmycWeb.Models
{
    public class Document
    {
        [Key]
        public string DocumentId { get; set; }
        public byte[] Content { get; set; }
        public string DocumentName { get; set; }

        [ScaffoldColumn(false)]
        public string ContentType { get; set; }

        public string Id { get; set; }
        [ForeignKey("Id")]
        public ApplicationUser User { get; set; }
    }
}
