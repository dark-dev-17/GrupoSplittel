using System;
using System.Collections.Generic;
using System.Text;

namespace GPSInformation.Reportes.ProduccionV3
{
    public class ReporteProd
    {
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public List<EmpleadoProd> EmpleadoProds { get; set; }
    }
}
