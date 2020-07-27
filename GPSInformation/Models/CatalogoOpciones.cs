using GPSInformation.Attributes;
using GPSInformation.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(Name = "CatalogoOpciones", IsMappedByLabels = true,IsStoreProcedure = false)]
    public class CatalogoOpciones
    {
        [ColumnDB(Name = "IdCatalogoOpciones", IsMapped = true,IsKey =true)]
        public int IdCatalogoOpciones { get; set; }
        [Required]
        [ColumnDB(Name = "Descripcion", IsMapped = true, IsKey = false)]
        public string Descripcion { get; set; }
    }
}
