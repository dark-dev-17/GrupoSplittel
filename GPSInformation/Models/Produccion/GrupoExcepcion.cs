using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSInformation.Models.Produccion
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class GrupoExcepcion
    {
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdGrupoExcepcion { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Fecha { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public bool EsNoche { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public TimeSpan Entrada { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public double HorasMeta { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public double HorasReal { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Comentarios { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPersona { get; set; }
    }
}
