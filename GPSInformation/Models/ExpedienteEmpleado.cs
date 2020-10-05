using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class ExpedienteEmpleado
    {
        [Display(Name = "#")]
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdExpedienteEmpleado { get; set; }

        [Required]
        [Display(Name = "Archivo")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdExpedienteArchivo { get; set; }

        [Required]
        [Display(Name = "Ruta del arhivo")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Ruta { get; set; }

        [Required]
        [Display(Name = "Tipo")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string TipoFile { get; set; }

        [Required]
        [Display(Name = "Empleado")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPersona { get; set; }

        [Required]
        [Display(Name = "Creado")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Creado { get; set; }

        [Required]
        [Display(Name = "Actualizado")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Actualizado { get; set; }
    }

    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class ExpedienteArchivo
    {
        [Display(Name = "#")]
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdExpedienteArchivo { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Nombre { get; set; }
    }

}
