using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionPersonal.Models
{
    public class EmpleadoInfor
    {
        public GPSInformation.Models.Persona persona { get; set; }
        public GPSInformation.Models.Puesto Puesto { get; set; }
        public GPSInformation.Models.Empleado Empleado { get; set; }
    }
}
