using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSInformation.Models.Produccion
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class GrupoProdCorte
    {
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdGrupoProdCorte { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPersona { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime FechaCorte { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Comentarios { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public double HorasMeta { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public double HorasReal { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public double HorasJusti { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public double Score { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Creado { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Modificado { get; set; }
    }
}
