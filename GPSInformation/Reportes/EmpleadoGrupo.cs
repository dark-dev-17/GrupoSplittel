using System;
using System.Collections.Generic;
using System.Text;

namespace GPSInformation.Reportes
{
    public class EmpleadoGrupo
    {
        public int NoSemana { get; set; }
        public int year_ { get; set; }
        public List<EmpleadoGr> Personas { get; set; }
    }

    public class EmpleadoGr
    {
        public int IdPersona { get; set; }
        public int IdGrupo { get; set; }
    }
}
