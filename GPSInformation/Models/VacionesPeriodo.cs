using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class VacionesPeriodo
    {
        [Display(Name = "ID")]
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdVacionesPeriodo { get; set; }

        [Required]
        [Display(Name = "Solicitante")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPersona { get; set; }

        [Required]
        [Display(Name = "Perido")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int NumeroPeriodo { get; set; }

        [Required]
        [Display(Name = "Dias Aprobados")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int DiasAprobadors { get; set; }

        [Required]
        [Display(Name = "DiasUsados")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public double DiasUsados { get; set; }
        
        [Required]
        [Display(Name = "Completo")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public bool Completo { get; set; }

        [Required]
        [Display(Name = "Creado")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Creado { get; set; }

        [Required]
        [Display(Name = "Actualizado")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Actualizado { get; set; }
    }
}
