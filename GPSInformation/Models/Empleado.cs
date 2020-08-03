using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(Name = "InformacionMedica", IsMappedByLabels = false, IsStoreProcedure = false)]
    public class Empleado
    {
        [ColumnDB(Name = "IdPersona", IsMapped = true, IsKey = true)]
        public int IdEmpleado { get; set; }
        [Required]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public int IdPersona { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:000000}", ApplyFormatInEditMode = true)]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public int NumeroNomina { get; set; }
        [Required]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public int TipoNomina { get; set; }
        [Required]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public int IdSociedad { get; set; }
        [Required]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public int IdDepartamento { get; set; }
        [Required]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public int IdPuesto { get; set; }
        [Required]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public DateTime Ingreso { get; set; }
        [Required]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public DateTime Egreso { get; set; }
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public string Email { get; set; }
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public string Extension { get; set; }
        [Required]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public double Salario { set; get; }
        [Required]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public int IdEstatus { get; set; }
    }
}
