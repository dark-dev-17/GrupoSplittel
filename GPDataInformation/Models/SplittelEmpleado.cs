using System;
using System.Collections.Generic;
using System.Text;

namespace GPDataInformation.Models
{
    public class SplittelEmpleado
    {
        public Persona persona { get;  set; }
        public Empleado empleado { get; internal set; }
        public InformacionMedica informacionMedica { get; internal set; }
        private PersonaContacto personaContacto { get;  set; }
        public List<PersonaContacto> PersonaContacto { get; internal set; }
        public DBConnection dBConnection { get; set; }
        public SplittelEmpleado()
        {
            this.persona = new Persona(this.dBConnection);
            persona.Nacimiento = DateTime.Now;
            persona.Nacimiento = persona.Nacimiento.AddYears(-20);
            this.empleado = new Empleado(this.dBConnection);
            this.informacionMedica = new InformacionMedica(this.dBConnection);
            this.personaContacto = new PersonaContacto(this.dBConnection);
        }
        public SplittelEmpleado(DBConnection dBConnection)
        {
            this.dBConnection = dBConnection;
            this.persona = new Persona(this.dBConnection);
            this.empleado = new Empleado(this.dBConnection);
            this.informacionMedica = new InformacionMedica(this.dBConnection);
            this.personaContacto = new PersonaContacto(this.dBConnection);

        }

        public SplittelEmpleado Get(int? idPersona)
        {
            persona = persona.Get(idPersona);
            if (persona != null)
            {
                var emp = empleado.Get(idPersona);
                if(emp != null)
                {
                    empleado = emp;
                    var medica = informacionMedica.Get(idPersona);
                    if (medica != null)
                    {
                        informacionMedica = medica;
                        PersonaContacto = personaContacto.GetList(idPersona);
                    }
                    else
                    {
                        informacionMedica = new InformacionMedica();
                    }
                }
                else
                {
                    empleado = new Empleado();
                }
                return this;
            }
            else
            {
                return null;
            }
        }

        public void SetConnection(DBConnection dBConnection)
        {
            this.dBConnection = dBConnection;
            this.persona = new Persona(this.dBConnection);
            this.empleado = new Empleado(this.dBConnection);
            this.informacionMedica = new InformacionMedica(this.dBConnection);
            this.personaContacto = new PersonaContacto(this.dBConnection);
        }
    }
}
