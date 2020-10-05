using System;
using System.Collections.Generic;
using System.Text;

namespace GPSInformation.Tools
{
    public static class Funciones
    {
        public static double GetAntiguedad(DateTime fechaInicio)
        {
            TimeSpan antiguedad = DateTime.Now - fechaInicio;
            return antiguedad.TotalDays / 365;
        }
    }
}
