using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Views
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class EnsablesTurnos
    {
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPersona { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public string NombreCompleto { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdTurnosProduccion { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdTurnoEmpleado { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Fecha { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public TimeSpan Inicio { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public TimeSpan Fin { get; set; }
        
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string DescripcionTurno { get; set; }
    }
}
