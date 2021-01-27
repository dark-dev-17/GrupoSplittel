using GPSInformation.Models.Produccion;
using GPSInformation.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Reportes.Produccion
{
    /*
     1.- Listar empleados
     2.- obtener ultimo turno
     3.- obtener reporte
     */
    public class EmpleadoProduccion
    {
        [Display(Name = "#")]
        public int IdPersona { get; set; }
        [Display(Name = "No.Nomina")]
        public string NumeroNomina { get; set; }
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }
        public string Puesto { get; set; }
        public double Antiguedad { get; set; }
        public double HorasTrabajadas { get; set; }
        public double HorasMeta { get; set; }
        public List<DiaEmpleadoProd> Dias { get; set; }
    }

    public class DiaEmpleadoProd
    {
        //public TipoDia TipoDia { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Entrada { get; set; }
        public DateTime Salida { get; set; }
        public DateTime R_Entrada { get; set; }
        public DateTime R_Salida { get; set; }
        public double HorasAprobadas { get; set; }
        public double HorasMeta { get; set; }
        public double HorasReal { get; set; }
        public GrupoHorario GrupoHorario { get; set; }
        public GrupoExcepcion GrupoExcepcion { get; set; }
        public List<View_gps_ensambleSinFiltro> Logs { get; set; }
    }

    public class ReporteProdEmp
    {
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public List<EmpleadoProduccion> Empleados { get; set; }
    }

    public enum TipoDia
    {
        Trabajo = 1,
        Descanso = 2,
        CruceTurno = 3,
        SinGrupo = 4,
        Otro = 5
    }
}
