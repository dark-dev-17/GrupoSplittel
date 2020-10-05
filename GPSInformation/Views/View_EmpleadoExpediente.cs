using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Views
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class View_EmpleadoExpediente
    {
        [Display(Name = "#")]
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdExpedienteEmpleado { get; set; }

        [Display(Name = "Archivo")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdExpedienteArchivo { get; set; }

        [Display(Name = "Ruta del arhivo")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Ruta { get; set; }

        [Display(Name = "Tipo")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string TipoFile { get; set; }

        [Display(Name = "Empleado")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPersona { get; set; }

        [Display(Name = "Creado")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Creado { get; set; }

        [Display(Name = "Actualizado")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Actualizado { get; set; }

        [Display(Name = "Nombre")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Nombre { get; set; }
    }
}
