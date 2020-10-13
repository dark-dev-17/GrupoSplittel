using GPSInformation.Attributes;
using GPSInformation.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class TurnosProduccion
    {
        [Display(Name = "#")]
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdTurnosProduccion { get; set; }

        [Required]
        [Display(Name = "Descripcion")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Descripcion { get; set; }

        [Required]
        [Display(Name = "Entrada")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public TimeSpan Entrada { get; set; }

        [Required]
        [Display(Name = "Salida")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public TimeSpan Salida { get; set; }
    }
}
