using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Slide
    {
        public int Id { get; set; }
        public string Url { get; set; }
        [NotMapped,Required]
        public IFormFile Image { get; set; }
    }
}
