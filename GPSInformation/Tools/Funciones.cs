using GPSInformation.Controllers;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

        public static List<Registro> GetRegistrosInc()
        {
            List<Registro>  Nomenclatura = new List<Registro>();
            Nomenclatura.Add(new Registro { Clave = "S.E.5", Title = "Salario Emocional", TextColor = "#ffffff", Color = "#ffff00", Tipo = 1 });
            Nomenclatura.Add(new Registro { Clave = "VAC", Title = "Vacacione", TextColor = "#ffffff", Color = "#2962ff", Tipo = 1 });
            Nomenclatura.Add(new Registro { Clave = "PCG", Title = "Permiso con goce", TextColor = "#ffffff", Color = "#bf360c", Tipo = 1 });
            Nomenclatura.Add(new Registro { Clave = "VIS", Title = "Visitas a Cliente", TextColor = "#ffffff", Color = "#bdbdbd", Tipo = 1 });
            Nomenclatura.Add(new Registro { Clave = "FAL", Title = "Falta Injustificado", TextColor = "#ffffff", Color = "#0277bd", Tipo = 1 });
            Nomenclatura.Add(new Registro { Clave = "INC", Title = "Incapacidad Medico", TextColor = "#ffffff", Color = "#bf360c", Tipo = 1 });
            Nomenclatura.Add(new Registro { Clave = "GS", Title = "Guardia Sabatino", TextColor = "#000000", Color = "#e0e0e0", Tipo = 1 });
            Nomenclatura.Add(new Registro { Clave = "DXG", Title = "Descanso por Guardia Sabatina Horario Flotado", TextColor = "#ffffff", Color = "#aa00ff", Tipo = 1 });
            Nomenclatura.Add(new Registro { Clave = "DXC", Title = "Descanso por Cumpleaños", TextColor = "#ffffff", Color = "#009688", Tipo = 1 });
            Nomenclatura.Add(new Registro { Clave = "FES", Title = "Festivo", TextColor = "#ffffff", Color = "#76ff03", Tipo = 1 });
            Nomenclatura.Add(new Registro { Clave = "PSG", Title = "permiso sin goce de sueldo", TextColor = "#ffffff", Color = "#9ccc65", Tipo = 1 });
            Nomenclatura.Add(new Registro { Clave = "TXT", Title = "Tiempo x Tiempo", TextColor = "#ffffff", Color = "#f1948a ", Tipo = 1 });
            Nomenclatura.Add(new Registro { Clave = "SAL", Title = "SALIDA POR TEMA DE TRABAJO O CURSO", TextColor = "#ffffff", Color = "#aeea00", Tipo = 1 });
            Nomenclatura.Add(new Registro { Clave = "HO", Title = "Home Office", TextColor = "#ffffff", Color = "#186a3b", Tipo = 1 });
            Nomenclatura.Add(new Registro { Clave = "SNJ", Title = "Incidencia sin justificar", TextColor = "#ffffff", Color = "#dd2c00", Tipo = 1 });
            Nomenclatura.Add(new Registro { Clave = "INJ", Title = "Incidencia justificada", TextColor = "#ffffff", Color = "#000000", Tipo = 1 });
            Nomenclatura.Add(new Registro { Clave = "DES", Title = "Descanso", TextColor = "#fd4d45", Color = "#000000", Tipo = 1 });


            return Nomenclatura;
        }
    }
}
