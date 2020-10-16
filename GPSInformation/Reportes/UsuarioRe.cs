using GPSInformation.Models;
using GPSInformation.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSInformation.Reportes
{
    public class UsuarioRe
    {
        public Usuario Usuario { get; set; }
        public View_empleado view_Empleado { get; set; }
        public string Ipserver { get; set; }
        public string Port { get; set; }
    }

    public class IncidenciaPermisoRe
    {
        public View_empleado view_Empleado { get; set; }
        public IncidenciaPermiso IncidenciaPermiso { get; set; }
        public string Asunto { get; set; }
        public string PagoPermiso { get; set; }
        public bool ModeAmin { get; set; }
    }

    public class IncidenciaVacaRe
    {
        public View_empleado view_Empleado { get; set; }
        public IncidenciaVacacion IncidenciaVacacion { get; set; }
        public bool ModeAmin { get; set; }
    }
}
