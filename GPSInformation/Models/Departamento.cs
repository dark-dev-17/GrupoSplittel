using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(Name = "Departamento", IsMappedByLabels = false, IsStoreProcedure = true)]
    public class Departamento
    {
        [ColumnDB(Name = "IdDepartamento", IsMapped = true, IsKey = true)]
        public int IdDepartamento { get; set; }
        [Required]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public string Nombre { get; set; }
        [Required]
        [ColumnDB(Name = "IdDireccion", IsMapped = true, IsKey = false)]
        public int IdDireccion { get; set; }
        [ColumnDB(Name = "Direccion", IsMapped = false, IsKey = false)]
        public Direccion Direccion { get; set; }
    }
}
