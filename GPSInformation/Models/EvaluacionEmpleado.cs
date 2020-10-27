using GPSInformation.Attributes;
using GPSInformation.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class EvaluacionEmpleado
    {
        [Display(Name = "Folio")]
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdEvaluacionEmpleado { get; set; }

        [Required]
        [Display(Name = "Folio")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdEvaluacion { get; set; }

        [Required]
        [Display(Name = "Folio")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPersona { get; set; }

        [Display(Name = "Folio")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public bool Respondio { get; set; }

        [Display(Name = "Folio")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime? Enviada { get; set; }

        [Display(Name = "Folio")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime? Contestada { get; set; }

        [ColumnDB(IsMapped = false, IsKey = false)]
        public View_empleado EmpleadoDatos { get; set; }

        [ColumnDB(IsMapped = false, IsKey = false)]
        public List<int> Participantes { get; set; }

    }
}
