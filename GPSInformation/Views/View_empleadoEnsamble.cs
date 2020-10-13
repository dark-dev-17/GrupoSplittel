using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Views
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class View_empleadoEnsamble
    {
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdPersona { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string NombreCompleto { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Correo { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime Ingreso { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public string PuestoNombre { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public string NumeroNomina { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public string TipoNomina { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public string NombreSociedad { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdDepartamento { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public string NombreDepartamento { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdTipoNomina { get; set; }

        [ColumnDB(IsMapped = false, IsKey = false)]
        [DisplayFormat(DataFormatString = "{0:00}", ApplyFormatInEditMode = false)]
        public double Antiguedad { get { return Tools.Funciones.GetAntiguedad(Ingreso); } }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPuesto { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdEstatus { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public string EstatusDescripcion { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdTurnosProduccion { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public string DescripcionTurno { get; set; }
        
        [ColumnDB(IsMapped = true, IsKey = false)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaFinturno { get; set; }
    }
}
