using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(Name = "CatalogoOpcionesValores", IsMappedByLabels = true, IsStoreProcedure = false)]
    public class CatalogoOpcionesValores
    {
        [ColumnDB(Name = "IdCatalogoOpcionesValores", IsMapped = true, IsKey = true)]
        public int IdCatalogoOpcionesValores { get; set; }
        [Required]
        [Display(Name ="Descripción")]
        [ColumnDB(Name = "Descripcion", IsMapped = true, IsKey = false)]
        public string Descripcion { get; set; }
        [Required]
        [ColumnDB(Name = "IdCatalogoOpciones", IsMapped = true, IsKey = false)]
        public int IdCatalogoOpciones { get; set; }
    }
}
