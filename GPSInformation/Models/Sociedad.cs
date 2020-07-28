using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(Name = "Sociedad", IsMappedByLabels = false, IsStoreProcedure = true)]
    public class Sociedad
    {
        [ColumnDB(Name = "IdSociedad", IsMapped = true, IsKey = true)]
        public int IdSociedad { get; set; }
        [Required]
        [ColumnDB(Name = "Descripcion", IsMapped = true, IsKey = false)]
        public string Descripcion { get; set; }
        [Required]
        [ColumnDB(Name = "Direccion", IsMapped = true, IsKey = false)]
        public string Direccion { get; set; }
    }
}
