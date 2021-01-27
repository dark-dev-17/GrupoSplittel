using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSInformation.Models.Produccion
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class GrupoProduccionAsi
    {
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdGrupoProduccionAsi { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdGrupo { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public int Year { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPersona { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Comentarios { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public int NoSemana { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Inicio { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Fin { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public double HrsTrabaja { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public double HrsMeta { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Creado { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Modificado { get; set; }

        [ColumnDB(IsMapped = false, IsKey = false)]
        public string GrupoNombre { get; set; }
    }
}
