using GPSInformation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Views
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class View_empleado
    {
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdPersona { get; set; }
        [ColumnDB(IsMapped = true, IsKey = false)]
        public string NombreCompleto { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Correo { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
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
        public double Antiguedad { get { return Tools.Funciones.GetAntiguedad(Ingreso); } }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdPuesto { get; set; }
    }
}
