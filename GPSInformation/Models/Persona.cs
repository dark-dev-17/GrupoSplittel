using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(Name = "Persona", IsMappedByLabels = false, IsStoreProcedure = false)]
    public class Persona
    {
        [Display(Name = "#")]
        [ColumnDB(Name = "IdPersona", IsMapped = true, IsKey = true)]
        public int IdPersona { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public string Nombre { get; set; }

        [Display(Name = "Apellido Paterno")]
        [Required]
        [ColumnDB(Name = "ApellidoPaterno", IsMapped = true, IsKey = false)]
        public string ApellidoPaterno { get; set; }

        [Display(Name = "Apellido Materno")]
        [Required]
        [ColumnDB(Name = "ApellidoMaterno", IsMapped = true, IsKey = false)]
        public string ApellidoMaterno { get; set; }

        [Display(Name = "Fe.Nacimiento")]
        [Required]
        [ColumnDB(Name = "Nacimiento", IsMapped = true, IsKey = false)]
        public DateTime Nacimiento { get; set; }

        [Display(Name = "Genero")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Por favor selecciona una opción")]
        [ColumnDB(Name = "IdGenero", IsMapped = true, IsKey = false)]
        public int IdGenero { get; set; }

        [Display(Name = "Estado civil")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Por favor selecciona una opción")]
        [ColumnDB(Name = "IdEstadoCivil", IsMapped = true, IsKey = false)]
        public int IdEstadoCivil { get; set; }

        [Display(Name = "RFC")]
        [Required]
        [ColumnDB(Name = "RFC", IsMapped = true, IsKey = false)]
        public string RFC { get; set; }

        [Display(Name = "CURP")]
        [Required]
        [ColumnDB(Name = "CURP", IsMapped = true, IsKey = false)]
        public string CURP { get; set; }

        [Display(Name = "Correo personal")]
        [Required]
        [ColumnDB(Name = "Email", IsMapped = true, IsKey = false)]
        public string Email { get; set; }

        [Display(Name = "Telefono personal")]
        [Required]
        [ColumnDB(Name = "TelefonoPersonal", IsMapped = true, IsKey = false)]
        public string TelefonoPersonal { get; set; }

        [Display(Name = "Telefono casa")]
        [Required]
        [ColumnDB(Name = "TelefonoFijo", IsMapped = true, IsKey = false)]
        public string TelefonoFijo { get; set; }

        [Display(Name = "Codigo postal")]
        [Required]
        [ColumnDB(Name = "CodigoPostal", IsMapped = true, IsKey = false)]
        public string CodigoPostal { get; set; }

        [Display(Name = "Colonia")]
        [Required]
        [ColumnDB(Name = "Colonia", IsMapped = true, IsKey = false)]
        public string Colonia { get; set; }

        [Display(Name = "Calle")]
        [Required]
        [ColumnDB(Name = "Calle", IsMapped = true, IsKey = false)]
        public string Calle { get; set; }

        [Display(Name = "Empleado")]

        [ColumnDB(Name = "Empleado", IsMapped = true, IsKey = false)]
        public int Empleado { get; set; }

        [ColumnDB(Name = "Creado", IsMapped = true, IsKey = false)]
        public DateTime Creado { get; set; }

        [ColumnDB(Name = "Actualizado", IsMapped = true, IsKey = false)]
        public DateTime Actualizado { get; set; }

        [Required]
        [Display(Name = "NSS")]
        [ColumnDB(Name = "NSS", IsMapped = true, IsKey = false)]
        public string NSS { get; set; }

        [Display(Name = "Puesto a aplicar")]
        [ColumnDB(Name = "IdPuesto", IsMapped = true, IsKey = false)]
        public int IdPuesto { get; set; }

        [Display(Name = "Estatus del prospecto")]
        [ColumnDB(Name = "Estatus", IsMapped = true, IsKey = false)]
        public int IdEstatusPros { get; set; }

        [Display(Name = "Nombre Completo")]
        [ColumnDB(IsMapped = false, IsKey = false)]
        public string NombreCompelto { get { return string.Format("{0} {1} {2}",Nombre,ApellidoPaterno,ApellidoMaterno); } }
    }
}
