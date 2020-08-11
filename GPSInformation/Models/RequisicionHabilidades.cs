using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class RequisicionHabilidades
    {
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdRequisicionHabilidades { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdRequisicionPersonal { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Modificado { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int Bloque { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdHabilidad { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Descripcion { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public bool Selected { get; set; }
        [ColumnDB(IsMapped = false, IsKey = false)]
        public int Posicion { get; set; }
    }
}
