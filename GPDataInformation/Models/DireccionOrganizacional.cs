using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPDataInformation.Models
{
    class DireccionOrganizacional
    {
        public int IdDireccion { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        [Display(Name = "Sociedad")]
        public int IdSociedad { get; set; }
        public Sociedad Sociedad { get; set; }
        [Display(Name = "Direccion parent")]
        public int DireccionParent { get; set; }
        public DireccionOrganizacional DireccionPa { get; set; }
    }
}
