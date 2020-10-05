using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceFibremexApi.Models
{
    public class Login
    {
        [Required]
        public string User { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
