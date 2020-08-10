using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class RequisicionPersonal
    {
        [DisplayFormat(DataFormatString = "R{0:0000}", ApplyFormatInEditMode = true)]
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdRequisicionPersonal { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPersona { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Fecha { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string NombreCompleto { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Departamento { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Por favor selecciona una opción")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int TipoRequisicion { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Por favor selecciona una opción")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int Motivo { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string OtroMotivo { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string PersonaSustituir { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Por favor selecciona una opción")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPuesto { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Escolaridad { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string EquipoMaquinaria { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Idiomas { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string SistemasProgramas { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Experiencia { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Por favor selecciona una opción")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int Sexo { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Por favor selecciona una opción")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int EstadoCivil { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string RangoEdad { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int PasoCompletado { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPersonaAprove { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime? FechaAprove { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Comentarios { get; set; }
    }
}
