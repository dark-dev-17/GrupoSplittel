using GPSInformation.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSInformation.Reportes
{
    public class ContratoEmp
    {
        public Persona Persona { get; set; }
        public Puesto Puesto { get; set; }
        public Empleado Empleado { get; set; }
        public InformacionCompania InformacionCompania { get; set; }
        public EmpleadoContrato EmpleadoContrato { get; set; }
    }
}
