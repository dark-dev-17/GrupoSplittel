using GPSInformation.Attributes;
using GPSInformation.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class SalaReservacion
    {
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdSalaReservacion { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdSala { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Motivo { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Comentarios { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime FechaInicio { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public TimeSpan HoraIncio { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public TimeSpan HolaFin { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPersona { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public bool Activa { get; set; }
    }


}
