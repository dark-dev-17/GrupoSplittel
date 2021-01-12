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
        [Display(Name = "#")]
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdOrganigramaStructura { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]

        [Display(Name = "Creado")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Version")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdOrganigramaVersion { get; set; }

        [Display(Name = "Puesto")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPuesto { get; set; }

        [Display(Name = "DPU")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string DPU { get; set; }

        [Display(Name = "Descripción")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Descripcion { get; set; }

        [Display(Name = "Puesto superior")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPuestoParent { get; set; }

        [Display(Name = "Nivel")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int Nivel { get; set; }

    }

    
}
