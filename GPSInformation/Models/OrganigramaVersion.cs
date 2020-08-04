using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class OrganigramaVersion
    {
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdOrganigramaVersion { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime FechaCreacion { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime FechaActualizacion { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int Autirizada { get; set; }
    }
}
