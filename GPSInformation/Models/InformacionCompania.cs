using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class InformacionCompania
    {
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdInformacionCompania { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Nombre { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Encargado { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Direccion { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public int CodigoPostal { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Enfoque { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public bool Activa { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Creada { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Actualizada { get; set; }
    }
}
