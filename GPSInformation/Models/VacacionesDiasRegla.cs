using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class VacacionesDiasRegla
    {
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdVacacionesDiasRegla { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int NoAnio { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int NoDias { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Registrado { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Modificado { get; set; }
    }

    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class DiaFeriado
    {
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdDiaFeriado { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Fecha { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Descripcion { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public bool Forzado { get; set; }

    }
}
