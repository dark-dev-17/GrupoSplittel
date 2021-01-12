using GPSInformation.Attributes;
using GPSInformation.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{

    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class TurnoEmpleado
    {
        [Display(Name = "#")]
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdTurnoEmpleado { get; set; }

        [Required]
        [Display(Name = "Empleado")]
        [Range(1, int.MaxValue, ErrorMessage = "Por favor selecciona una opción")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPersona { get; set; }

        [Required]
        [Display(Name = "Turno")]
        [Range(1, int.MaxValue, ErrorMessage = "Por favor selecciona una opción")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdTurnosProduccion { get; set; }

        [Required]
        [Display(Name = "Inicio")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Fecha { get; set; }

        [Display(Name = "Inicio")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public TimeSpan Inicio { get; set; }

        [Display(Name = "Fin")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public TimeSpan Fin { get; set; }

        [Display(Name = "Fin")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public bool Activo { get; set; }

    }

    public class TurnoEmpleadoForm
    {
        [Required]
        [Display(Name = "Empleado")]
        [Range(1, int.MaxValue, ErrorMessage = "Por favor selecciona una opción")]
        public int IdPersona { get; set; }

        [Required]
        [Display(Name = "Turno")]
        [Range(1, int.MaxValue, ErrorMessage = "Por favor selecciona una opción")]
        public int IdTurnosProduccion { get; set; }

        [Required]
        [Display(Name = "Inicio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaInicio { get; set; }

        [Required]
        [Display(Name = "Inicio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaFin { get; set; }
    }
    public class TurnoProdForm
    {
        [Required]
        [Display(Name = "Empleado")]
        [Range(1, int.MaxValue, ErrorMessage = "Por favor selecciona una opción")]
        public int IdPersona { get; set; }

        [Required]
        [Display(Name = "Grupo")]
        [Range(1, int.MaxValue, ErrorMessage = "Por favor selecciona una opción")]
        public int IdGrupo{ get; set; }

        [Required]
        [Display(Name = "Inicio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaInicio { get; set; }

        [Required]
        [Display(Name = "Inicio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaFin { get; set; }
    }
}
