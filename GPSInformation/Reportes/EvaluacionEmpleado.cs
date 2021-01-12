using GPSInformation.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSInformation.Reportes
{
    public class EvaluacionEmpleados
    {
        public Evaluacion Evaluacion { get; set; }
        public Views.View_empleado View_empleado { get; set; }
        public string Ippublic { get; set; }
    }
}
