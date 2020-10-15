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
        [Display(Name = "#")]
        [ColumnDB(Name = "IdDepartamento", IsMapped = true, IsKey = true)]
        public int IdDepartamento { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public string Nombre { get; set; }

        [Display(Name = "Dirección")]
        [Required]
        [ColumnDB(Name = "IdDireccion", IsMapped = true, IsKey = false)]
        public int IdDireccion { get; set; }

        [Display(Name = "Direccion")]
        [ColumnDB(Name = "Direccion", IsMapped = false, IsKey = false)]
        public Direccion Direccion { get; set; }
        [ColumnDB(Name = "ClaveDPU", IsMapped = true, IsKey = false)]

        [Display(Name = "Clave DPU")]
        public string ClaveDPU { get; set; }
    }
}
