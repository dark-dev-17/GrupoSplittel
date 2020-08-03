using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class IncidenciaPermiso
    {
        [Display(Name = "Folio")]
        [DisplayFormat(DataFormatString = "{0:0000}", ApplyFormatInEditMode = true)]
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdIncidenciaPermiso { get; set; }
        [Required]
        [Display(Name = "Solicitante")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPersona { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Fecha { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public TimeSpan Inicio { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public TimeSpan Fin { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdAsunto { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string DescripcionAsunto { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPagoPermiso { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string CreadoPor { get; set; }

        [ColumnDB(IsMapped = false, IsKey = false)]
        public string EmpleadoNombre { get; set; }
        [ColumnDB(IsMapped = false, IsKey = false)]
        public string DEscripcionTipo { get; set; }
    }
}
