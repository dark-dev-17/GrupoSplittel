using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class FaltaJustificacion
    {
        [Display(Name = "#")]
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdFaltaJustificacion { get; set; }
        [Required]
        [Display(Name = "Archivo")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPersona { get; set; }
        [Required]
        [Display(Name = "Archivo")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Fecha { get; set; }
        [Required]
        [Display(Name = "Archivo")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Comentarios { get; set; }
        [Display(Name = "Archivo")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Creado { get; set; }
    }
}
