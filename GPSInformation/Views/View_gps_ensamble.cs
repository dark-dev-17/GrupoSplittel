using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Views
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class View_gps_ensambleSinFiltro
    {
        /// <summary>
        /// Departamento del colaborador
        /// </summary>
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string tDescDepartment { get; set; }

        /// <summary>
        /// Numero de nomina
        /// </summary>
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string tIdentification { get; set; }

        /// <summary>
        /// Apellidos
        /// </summary>
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string tLastName { get; set; }

        /// <summary>
        /// Nombre
        /// </summary>
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string tFirstName { get; set; }

        /// <summary>
        /// Numero de empleado en el sistema de control de acceso
        /// </summary>
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int iEmployeeNum { get; set; }

        /// <summary>
        /// Fecha del evento o registro
        /// </summary>
        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime dtEventReal { get; set; }

        /// <summary>
        /// Id del evento
        /// </summary>
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdAutoEvents { get; set; }

        /// <summary>
        /// Tipo de eevento
        /// </summary>
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int iEventType { get; set; }

        /// <summary>
        /// Salida o entrada
        /// </summary>
        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdReader { get; set; }

        /// <summary>
        /// Descripcion del evento traza
        /// </summary>
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string tDesc { get; set; }

        /// <summary>
        /// Salida o entrada
        /// </summary>
        [ColumnDB(IsMapped = false, IsKey = false)]
        public EnsamblesTipoChec TipoRegistro { get { return IdReader == 18 ? EnsamblesTipoChec.Entrada : IdReader == 17 ? EnsamblesTipoChec.Salida : EnsamblesTipoChec.otro; } }
    }

    public enum EnsamblesTipoChec
    {
        Entrada = 0,
        Salida = 1,
        otro = 3
    }
}
