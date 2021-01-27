using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSInformation.Models.Produccion
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class GrupoArreglo
    {
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdGrupoArreglo { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPersona { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdEvent { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime FechaHora { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public bool EsIgnorado { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Comentarios { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Creado { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Modificado { get; set; }
    }
}
