using GPSInformation.Attributes;
using GPSInformation.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(Name = "CatalogoOpciones", IsMappedByLabels = true)]
    public class CatalogoOpciones
    {
        [ColumnDB(Name = "IdCatalogoOpciones")]
        public int IdCatalogoOpciones { get; set; }
        [Required]
        [ColumnDB(Name = "Descripcion")]
        public string Descripcion { get; set; }
    }
}
