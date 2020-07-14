using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPS_Logic.Models
{
    public partial class Direccion
    {
        [Key]
        public int IdDireccion { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        [ForeignKey("FK__Direccion__IdSoc__15502E78")]
        [Display(Name = "Sociedad")]
        public int IdSociedad { get; set; }
        
        public Sociedad Sociedad { get; set; }
        [ForeignKey("FK_DireeccionDireccion")]
        [Display(Name = "Direccion parent")]
        public int DireccionParent { get; set; }
        public Direccion DireccionPa { get; set; }
    }
}
