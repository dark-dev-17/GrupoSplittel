using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class EmpleadoContrato
    {
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdEmpleadoContrato { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Inicio { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Fin { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        [Range(1, int.MaxValue, ErrorMessage = "Por favor selecciona una opción")]
        public int IdPersona { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        [Range(1, int.MaxValue, ErrorMessage = "Por favor selecciona una opción")]
        public int Tipo { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Created { get; set; }
    }
}
