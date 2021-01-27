using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSInformation.Models.Produccion
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class GrupoProdIncidencia
    {
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdGrupoProdIncidencia { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPersona { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public string TipoIncidecia { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public int TipoPermiso { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime FechaPermiso { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime FechaSalVac { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime FechaRegVac { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public double HorasJustific { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Comentarios { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Creado { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Modificado { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public string CreadoPor { get; set; }

        [ColumnDB(IsMapped = false, IsKey = false)]
        public string NameTipoPermiso { get; set; }
    }
}
