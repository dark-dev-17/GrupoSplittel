using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class IncidenciaVacacion
    {
        [Display(Name = "Folio")]
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdIncidenciaVacacion { get; set; }

        [Display(Name = "Solicitante")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPersona { get; set; }

        [Required]
        [Display(Name = "Inicio")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Inicio { get; set; }

        [Required]
        [Display(Name = "Fin")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Fin { get; set; }

        [Display(Name = "No.Días")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int NoDias { get; set; }

        [Display(Name = "Creado por")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string CreadoPor { get; set; }

        [Display(Name = "Estatus")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int Estatus { get; set; }

        [Display(Name = "No.Autorizaciones")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int NumAutorizaciones { get; set; }

        [Display(Name = "Tipo solicitud")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Tipo { get; set; }
    }

    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class IncidenciaVacacionProcess
    {
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdIncidenciaVacacionProcess { get; set; }

        [Display(Name = "IdIncidenciaVacacion")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdIncidenciaVacacion { get; set; }

        [Display(Name = "Aprobador")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPersona { get; set; }

        [Display(Name = "Fecha")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime? Fecha { get; set; }

        [Display(Name = "Comentarios")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Comentarios { get; set; }

        [Display(Name = "Fue revisada")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public bool Revisada { get; set; }

        [Display(Name = "Autorizada")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public bool Autorizada { get; set; }

        [Display(Name = "Nivel")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int Nivel { get; set; }

        [Display(Name = "Titulo")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Titulo { get; set; }

        [Display(Name = "NombreEmpleado")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string NombreEmpleado { get; set; }
    }
}
