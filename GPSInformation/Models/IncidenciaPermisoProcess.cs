using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace GPSInformation.Models
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class IncidenciaPermisoProcess
    {
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdIncidenciaPermisoProcess { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:0000}", ApplyFormatInEditMode = true)]
        [Display(Name = "Folio")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdIncidenciaPermiso { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Fecha { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPersona { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int Estatus { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Comentarios { get; set; }
    }
}
