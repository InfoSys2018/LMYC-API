using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LmycWeb.ViewModels
{
    public class DocumentViewModel
    {
        public IFormFile Content { get; set; }
    }
}
