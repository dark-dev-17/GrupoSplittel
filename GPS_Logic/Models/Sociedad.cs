using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GPS_Logic.Models
{
    public partial class Sociedad
    {
        public int IdSociedad { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public string Direccion { get; set; }
        public ICollection<Direccion> Direcciones { get; set; }
    }
}
