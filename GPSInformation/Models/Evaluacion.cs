using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class Evaluacion
    {
        [DisplayFormat(DataFormatString = "E{0:0000}", ApplyFormatInEditMode = false)]
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdEvaluacion { get; set; }

        [Required]
        
        [Display(Name = "Nombre")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "IdPersona")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPersona { get; set; }

        [Required]
        [Display(Name = "InicioFecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime InicioFecha { get; set; }

        [Required]
        [Display(Name = "InicioHora")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public TimeSpan InicioHora { get; set; }

        [Required]
        [Display(Name = "FinFecha")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime FinFecha { get; set; }

        [Required]
        [Display(Name = "FinHora")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public TimeSpan FinHora { get; set; }

        [Required]
        [Display(Name = "Modalidad")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdModalidad { get; set; }

        [Required]
        [Display(Name = "Modelo")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdEvaluacionTemplate { get; set; }

        [Required]
        [Display(Name = "Activa")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public bool Activa { get; set; }

        [Required]
        [Display(Name = "Creada")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Creada { get; set; }

        [Required]
        [Display(Name = "Actualizada")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Actualizada { get; set; }

        [ColumnDB(IsMapped = false, IsKey = false)]
        public string PersonaName { get; set; }

        [ColumnDB(IsMapped = false, IsKey = false)]
        public string ModalidadName { get; set; }

        [ColumnDB(IsMapped = false, IsKey = false)]
        public string ModeloName { get; set; }
        
        [ColumnDB(IsMapped = false, IsKey = false)]
        public List<EvaluacioSeccion> secciones { get; set; }
    }

    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class EvaluacionSeccionPregnts
    {
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdEvaluacionSeccionPregnts { get; set; }

        [Required]
        [Display(Name = "Folio")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdEvaluacioSeccion { get; set; }

        [Required]
        [Display(Name = "Pregunta")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Pregunta { get; set; }

        [Required]
        [Display(Name = "Comentarios")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Comentarios { get; set; }

        [Required]
        [Display(Name = "Activa")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public bool Activa { get; set; }

        [Required]
        [Display(Name = "Tipo")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int Tipo { get; set; }

        [Required]
        [Display(Name = "Tipo")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int MaxCalificacion { get; set; }

        [ColumnDB(IsMapped = false, IsKey = false)]
        public EvaluacionRespuestas Respuesta { get; set; }

    }

    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class EvaluacioSeccion
    {
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdEvaluacioSeccion { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Descripcion")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Descripcion { get; set; }

        [Required]
        [Display(Name = "Modelo")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdEvaluacionTemplate { get; set; }

        [ColumnDB(IsMapped = false, IsKey = false)]
        public List<EvaluacionSeccionPregnts> Preguntas { get; set; }

    }

    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class EvaluacionTemplate
    {
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdEvaluacionTemplate { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Descripcion")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Descripcion { get; set; }
    }
}
