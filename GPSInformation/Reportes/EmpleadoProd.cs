using GPSInformation.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSInformation.Reportes
{

    public class EmpleadoProd
    {
        public int IdPersona { get; set; }
        public string NumeroNomina { get; set; }
        public string Nombre { get; set; }
        public string Puesto { get; set; }
        public List<DiaProd> Dias { get; set; }
        public double HorasTrabajadas { get; set; }
        public double HorasMeta { get; set; }
        public double Diferencia { get { return HorasMeta - HorasTrabajadas; } }
        public double Antiguedad { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public string GrupoName { get; internal set; }
    }

    public class DiaProd
    {
        public DateTime Dia { get; set; }
        public DateTime? Entrada { get; set; }
        public DateTime? Salida { get; set; }
        public List<string> Logs { get; set; }
        public bool IsLocated { get; set; }
        public bool EsDescanso { get; set; }
        public bool EsNoche { get; set; }
        public bool EsCruce { get; set; }
        public int IdGrupoHorario { get; set; }
        public string Turno { get; set; }
        public double Horas { get { return Salida != null && Entrada != null ? Funciones.DifFechashoras((DateTime)Salida, (DateTime)Entrada) : 0; } }
        public double HorasMeta { get; set; }
    }
}
