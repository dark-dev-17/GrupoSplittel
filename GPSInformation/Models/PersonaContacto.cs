using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class PersonaContacto
    {
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdPersonaContacto { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPersona { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string NombreCompleto { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdParentezco { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Telefono { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string CodigoPostal { get; set; }
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Direccion { get; set; }
    }
}
