using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(Name = "Sociedad", IsMappedByLabels = false, IsStoreProcedure = true)]
    public class Direccion
    {
        [Display(Name = "#")]
        [ColumnDB(Name = "IdDireccion", IsMapped = true, IsKey = true)]
        public int IdDireccion { get; set; }
        [Required]
        [Display(Name = "Nombre")]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public string Nombre { get; set; }
        [Required]
        [Display(Name = "Sociedad")]
        [ColumnDB(Name = "IdSociedad", IsMapped = true, IsKey = false)]
        public int IdSociedad { get; set; }
        [Display(Name = "Direccion parent")]
        [ColumnDB(Name = "DireccionParent", IsMapped = true, IsKey = false)]
        public int DireccionParent { get; set; }

        [ColumnDB(Name = "DireccionPa", IsMapped = false, IsKey = false)]
        public Direccion DireccionPa { get; set; }
        [ColumnDB(Name = "Sociedad", IsMapped = false, IsKey = false)]
        public Sociedad Sociedad { get; set; }
    }
}
