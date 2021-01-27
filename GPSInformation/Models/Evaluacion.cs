using GPSInformation.Attributes;
using GPSInformation.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class EvaluacionInstructor
    {
        [Display(Name = "Clave")]
        [DisplayFormat(DataFormatString = "E{0:0000}", ApplyFormatInEditMode = false)]
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdEvaluacionInstructor { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdEvaluacion { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPersona { get; set; }
    }
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class Evaluacion
    {
        [Display(Name = "Clave")]
        [DisplayFormat(DataFormatString = "E{0:0000}", ApplyFormatInEditMode = false)]
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdEvaluacion { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Nombre { get; set; }

        [Display(Name = "Ponente(s) interno(s)")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPersona { get; set; }

        [Required]
        [Display(Name = "Fe.Inicio")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime InicioFecha { get; set; }

        [Required]
        [Display(Name = "Hora de inicio")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public TimeSpan InicioHora { get; set; }

        [Required]
        [Display(Name = "Fe.Termino")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime FinFecha { get; set; }

        [Required]
        [Display(Name = "Hora de termino")]
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
        [Display(Name = "Evaluación impartida por externos")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public bool IsInterno { get; set; }

        [Display(Name = "Ponente(s) externo(s)")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string PonenteNameExt { get; set; }

        [Required]
        [Display(Name = "Actualizada")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Actualizada { get; set; }
        
        [Required]
        [Display(Name = "Duración(hrs) del curso o evaluación")]
        [Range(1, int.MaxValue, ErrorMessage = "Por favor introduce la duración en hrs del curso")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public double Duracion { get; set; }

        [ColumnDB(IsMapped = false, IsKey = false)]
        public string PersonaName { get; set; }

        [Display(Name = "Modalidad del curso o evaluación")]
        [ColumnDB(IsMapped = false, IsKey = false)]
        public string ModalidadName { get; set; }

        [ColumnDB(IsMapped = false, IsKey = false)]
        public string ModeloName { get; set; }
        
        [ColumnDB(IsMapped = false, IsKey = false)]
        public List<EvaluacioSeccion> secciones { get; set; }


        [ColumnDB(IsMapped = false, IsKey = false)]
        public EvaluacionEmpleado EvaluacionEmpleado { get; set; }

        [ColumnDB(IsMapped = false, IsKey = false)]
        public List<int> IdEmpleados { get; set; }

        [Display(Name = "Folio")]
        [ColumnDB(IsMapped = false, IsKey = false)]
        public string EncriptId { get { return EncryptData.Encrypt(IdEvaluacion + ""); } }
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
