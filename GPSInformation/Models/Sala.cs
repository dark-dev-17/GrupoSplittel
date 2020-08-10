using GPSInformation.Attributes;
using GPSInformation.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class Sala
    {
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdSala { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Nombre { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int Capacidad { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public bool Activa { get; set; }
    }
}
