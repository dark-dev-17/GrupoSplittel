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
        public bool IsActiveVersionOgg { get; set; }
        public bool IsPuestoOrg { get; set; }
    }

    public class EmpleadoInfor2
    {
        public GPSInformation.Models.Persona persona { get; set; }
        public PuestoOrg Puesto { get; set; }
        public GPSInformation.Models.Empleado Empleado { get; set; }
        public GPSInformation.Views.View_empleado View_empleado { get; set; }
        public PuestoOrg PuestoBoos { get; set; }
        public List<GPSInformation.Models.Persona> personaBoos { get; set; }
        public bool IsActiveVersionOgg { get; set; }
        public bool IsPuestoOrg { get; set; }
    }
}
