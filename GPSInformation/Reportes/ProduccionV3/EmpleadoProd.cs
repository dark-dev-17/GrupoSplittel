using GPSInformation.Models.Produccion;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSInformation.Reportes.ProduccionV3
{
    public class EmpleadoProd
    {
        public int IdPersona { get; set; }
        public string NombreCompleto { get; set; }
        public string NumeroNomina { get; set; }
        public string PuestoNombre { get; set; }
        public double Antiguedad { get; set; }
        public double HorasMeta { get; set; }
        public double HorasAprobadas { get; set; }
        public double HorasReal { get; set; }
        public DateTime Incio { get; set; }
        public DateTime Fin { get; set; }

        public List<AccessLog> Accessos { get; set; }
        public List<JornadaGrupo> JornadaGrupos { get; set; }
        public List<GrupoProdIncidencia> GrupoProdIncidencias { get; set; }
    }
}
