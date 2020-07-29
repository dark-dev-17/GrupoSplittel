using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(Name = "Persona", IsMappedByLabels = false, IsStoreProcedure = true)]
    public class Persona
    {
        [ColumnDB(Name = "IdPersona", IsMapped = true, IsKey = true)]
        public int IdPersona { get; set; }
        [Required]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public string Nombre { get; set; }
        [Required]
        [ColumnDB(Name = "ApellidoPaterno", IsMapped = true, IsKey = false)]
        public string ApellidoPaterno { get; set; }
        [Required]
        [ColumnDB(Name = "ApellidoMaterno", IsMapped = true, IsKey = false)]
        public string ApellidoMaterno { get; set; }
        [Required]
        [ColumnDB(Name = "Nacimiento", IsMapped = true, IsKey = false)]
        public DateTime Nacimiento { get; set; }
        [Required]
        [ColumnDB(Name = "IdGenero", IsMapped = true, IsKey = false)]
        public int IdGenero { get; set; }
        [Required]
        [ColumnDB(Name = "IdEstadoCivil", IsMapped = true, IsKey = false)]
        public int IdEstadoCivil { get; set; }
        [Required]
        [ColumnDB(Name = "RFC", IsMapped = true, IsKey = false)]
        public string RFC { get; set; }
        [Required]
        [ColumnDB(Name = "CURP", IsMapped = true, IsKey = false)]
        public string CURP { get; set; }
        [Required]
        [ColumnDB(Name = "Email", IsMapped = true, IsKey = false)]
        public string Email { get; set; }
        [Required]
        [ColumnDB(Name = "TelefonoPersonal", IsMapped = true, IsKey = false)]
        public string TelefonoPersonal { get; set; }
        [Required]
        [ColumnDB(Name = "TelefonoFijo", IsMapped = true, IsKey = false)]
        public string TelefonoFijo { get; set; }
        [Required]
        [ColumnDB(Name = "CodigoPostal", IsMapped = true, IsKey = false)]
        public string CodigoPostal { get; set; }
        [Required]
        [ColumnDB(Name = "Colonia", IsMapped = true, IsKey = false)]
        public string Colonia { get; set; }
        [Required]
        [ColumnDB(Name = "Calle", IsMapped = true, IsKey = false)]
        public string Calle { get; set; }
        [ColumnDB(Name = "Empleado", IsMapped = true, IsKey = false)]
        public int Empleado { get; set; }
    }
}
