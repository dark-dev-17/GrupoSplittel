using System;
using System.Collections.Generic;
using System.Text;

namespace GPDataInformation.Models
{
    public class SplittelEmpleado
    {
        public Persona persona { get;  set; }
        public Empleado empleado { get;  set; }
        public InformacionMedica informacionMedica { get; set; }
        public List<PersonaContacto> PersonaContacto { get;  set; }

        public SplittelEmpleado()
        {
            persona = new Persona();
            empleado = new Empleado();
            informacionMedica = new InformacionMedica();
            PersonaContacto = new List<PersonaContacto>();
        }
    }
}
