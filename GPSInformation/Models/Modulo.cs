using GPSInformation.Attributes;
using GPSInformation.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GPSInformation.Models
{
    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class Modulo
    {
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdModulo { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Nombre { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public int Posicion { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public bool Activo { get; set; }

        [ColumnDB(IsMapped = false, IsKey = false)]
        public List<SubModulo> SubModulos { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Icono { get; set; }

    }


    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class SubModulo
    {
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdSubModulo { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdModulo { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Nombre { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Descripcion { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Controllador { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public string Accion { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public int Tipo { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public int Posicion { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public bool Activo { get; set; }

        [ColumnDB(IsMapped = false, IsKey = false)]
        public AccesosSistema AccesosSistema { get; set; }

        [ColumnDB(IsMapped = false, IsKey = false)]
        public bool Activemenu { get; set; }

    }

    [TableDB(IsMappedByLabels = false, IsStoreProcedure = false)]
    public class AccesosSistema
    {
        [ColumnDB(IsMapped = true, IsKey = true)]
        public int IdAccesosSistema { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdUsuario { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public int IdSubModulo { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public bool TieneAcceso { get; set; }

        [ColumnDB(IsMapped = true, IsKey = false)]
        public DateTime Modificado { get; set; }
        
        [ColumnDB(IsMapped = true, IsKey = false)]
        public bool Forzado { get; set; }

    }
}
