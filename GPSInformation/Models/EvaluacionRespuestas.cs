using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
namespace GPSInformation.Models
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class EvaluacionRespuestas
    {
        [Display(Name = "Folio")]
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdEvaluacionRespuestas { get; set; }

        [Display(Name = "Folio")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdEvaluacion { get; set; }

        [Display(Name = "Folio")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPersona { get; set; }

        [Display(Name = "Folio")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdEvaluacionSeccionPregnts { get; set; }

        [Display(Name = "Folio")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Respuesta { get; set; }
    }
}
