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

        [Display(Name = "IdPersona")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Por favor selecciona una opción")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPersona { get; set; }

        [Display(Name = "Nombre completo")]
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string NombreCompleto { get; set; }

        [Display(Name = "Parentezco")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Por favor selecciona una opción")]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdParentezco { get; set; }

        [Display(Name = "Telefono")]
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Telefono { get; set; }

        [Display(Name = "Cod.Postal")]
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string CodigoPostal { get; set; }

        [Display(Name = "Dirección")]
        [Required]
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Direccion { get; set; }
    }
}
