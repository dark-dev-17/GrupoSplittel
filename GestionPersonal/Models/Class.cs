using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionPersonal.Models
{
    public class RegistrarUsuario
    {
        public int IdUsuario { get; set; }
        [Required]
        [Display(Name = "Número de nomina")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Contraseña")]
        public string Pass { get; set; }
        [Required]
        [Display(Name = "Vuelve a escribir contraseña")]
        public string Pass2 { get; set; }
        public bool Activo { get; set; }
        public DateTime UltimoIngreso { get; set; }
        public int IdPersona { get; set; }
    }
}
