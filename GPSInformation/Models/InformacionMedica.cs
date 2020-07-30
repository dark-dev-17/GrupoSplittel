using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(Name = "InformacionMedica", IsMappedByLabels = false, IsStoreProcedure = true)]
    public class InformacionMedica
    {
        [ColumnDB(Name = "IdPersona", IsMapped = true, IsKey = true)]
        public int IdInformacionMedica { get; set; }
        [Required]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public int IdPersona { get; set; }
        [Required]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public int TipoSangre { get; set; }
        [Required]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public int Alergias { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a number > 0")]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public double Altura { set; get; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a number > 0")]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public double Peso { set; get; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a number > 0")]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public double Talla { set; get; }
        [Required]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public double IMC { set; get; }
        [Required]
        [ColumnDB(Name = "Nombre", IsMapped = true, IsKey = false)]
        public string Comentarios { get; set; }
    }
}
