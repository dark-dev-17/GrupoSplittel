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
        [Display(Name = "Fecha del permiso")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Fecha { get; set; }

        [Display(Name = "Creado")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Creado { get; set; }

        [Required]
        [Display(Name = "Inicio")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public TimeSpan Inicio { get; set; }

        [Required]
        [Display(Name = "Fin")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public TimeSpan Fin { get; set; }

        [Required]
        [Display(Name = "Asunto")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdAsunto { get; set; }

        [Required]
        [Display(Name = "Estatus")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int Estatus { get; set; }

        [Required]
        [Display(Name = "Especifique asunto")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string DescripcionAsunto { get; set; }

        [Required]
        [Display(Name = "Tipo de permiso")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPagoPermiso { get; set; }

        [Display(Name = "Creado por")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string CreadoPor { get; set; }

        [ColumnDB(IsMapped = false, IsKey = false)]
        public string EmpleadoNombre { get; set; }

        [ColumnDB(IsMapped = false, IsKey = false)]
        public string DEscripcionTipo { get; set; }

        [ColumnDB(IsMapped = false, IsKey = false)]
        public List<IncidenciaProcess> Proceso { get; set; }

        [ColumnDB(IsMapped = false, IsKey = false)]
        public string Folio { get { return string.Format("P-{0:0000}", IdIncidenciaPermiso); } }
    }
}
