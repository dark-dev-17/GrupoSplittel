using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class OrganigramaStructura
    {
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdOrganigramaStructura { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime FechaCreacion { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdOrganigramaVersion { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPuesto { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string DPU { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Descripcion { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPuestoParent { get; set; }

    }

    
}
