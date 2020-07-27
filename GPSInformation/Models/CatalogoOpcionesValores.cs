using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    public class CatalogoOpcionesValores
    {
        public int IdCatalogoOpcionesValores { get; set; }
        [Required]
        [Display(Name ="Descripción")]
        public string Descripcion { get; set; }
        [Required]
        public int IdCatalogoOpciones { get; set; }
    }
}
