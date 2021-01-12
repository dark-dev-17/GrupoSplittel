using GPSInformation.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace GPSInformation.Reportes
{
    public class ColaboradorEnsamble
    {
        public string Nomina { get; set; }
        public string NombreCompleto { get; set; }
        //public List<DiaColaborador> Dias { get; set; }
        public List<GrupoDef> DefTurno { get; set; }
        public int SelectGroup { get; set; }
    }
    public class DiaColaborador
    {
        public int NumeroDia { get; set; }
        public DateTime? Entrada { get; set; }
        public DateTime? Salida { get; set; }
        public TimeSpan? DuracionHrs { get { return Salida != null ? Salida - Entrada : null;  } }
        public List<string> Incidencias { get; set; }
    }

    /*
        -Colaborador
        -   Turno
                Dia
                    ..
                    ..
                    ..
                    ..
            ..
            ..
            ..
     */
    public class GrupoDef
    {
        /// <summary>
        /// Tipo de grupo
        /// </summary>
        public GrupoTurno Tipo { get; set; }
        /// <summary>
        /// Listado de dias y horarios por grupo
        /// </summary>
        public List<TurnoDia> DiasHorarios { get; set; }
    }

    public class TurnoDia
    {
        public DateTime FechaDia { get; set; }
        /// <summary>
        /// aplica cuando es turno de noche
        /// </summary>
        public bool IsCrossDay { get; set; }
        /// <summary>
        /// es dia entre cambio de turno (dia -> noche / noche -> dia) 
        /// </summary>
        public bool EsCambio { get; set; }
        /// <summary>
        /// Dia de la semana
        /// </summary>
        public DayOfWeek Dia { get; set; }
        /// <summary>
        /// Aplica para descanso
        /// </summary>
        public bool EsDescanso { get; set; }
        /// <summary>
        /// Entrda del turno
        /// </summary>
        public TimeSpan Entrada { get; set; }
        /// <summary>
        /// Salida del turno
        /// </summary>
        public TimeSpan Salida { get; set; }
        /// <summary>
        /// Duracion en hrs del turno
        /// </summary>
        public TimeSpan? Horas { get { return Salida - Entrada; } }

        public List<LogDiaTurno> Accesos { get; set; }
        /// <summary>
        /// Hora de entrada y salida del dia laborado
        /// </summary>
        public TurnoDiareporte Infordia { get; set;  }


       
    }

    public class TurnoDiareporte
    {
        [DisplayFormat(DataFormatString = "{0:HH:mm:ss}")]
        public DateTime? Entrada { get; set; }
        [DisplayFormat(DataFormatString = "{0:HH:mm:ss}")]
        public DateTime? Salida { get; set; }
        public TimeSpan? DuracionHrs { get { return Salida != null ? Salida - Entrada : null; } }
        public string Logdescripcion { get; set; }
    }

    public class LogDiaTurno
    {
        /// <summary>
        /// Hora y tiempo del evento registrado
        /// </summary>
        public DateTime Re_time { get; set; }
        /// <summary>
        /// Descripcion del evento
        /// </summary>
        public string Descripccion { get; set; }
        /// <summary>
        /// Tipo de registro(Control de accesos)
        /// </summary>
        public EnsamblesTipoChec TipoLog { get; set; }
    }

    public class GrupoOrder
    {
        public int Indice { get; set; }
        public int total { get; set; }
    }

    public enum GrupoTurno
    {
        Gris = 1,
        Rojo = 2,
        Verde = 3
    }
}
