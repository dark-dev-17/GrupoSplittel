using System;
using System.Collections.Generic;
using System.Text;

namespace GPSInformation.Reportes.ProduccionV3
{
    public class AccessLog
    {
        public int IdAccessLog { get; set; }
        public DateTime Fecha { get; set; }
        public bool Activo { get; set; }
        public bool Forzado { get; set; }
        public string Descripcion { get; set; }
        public int IdEventChec { get; set; }
        public int Position { get; set; }
        public int IdGrupoArreglo { get; set; }
        public TipoAcceso TipoAcceso { get { return (Position % 2) == 0 ? TipoAcceso.Salida : TipoAcceso.Entrada; } }
    }

    public class JornadaGrupo
    {
        public int IdGrupo { get; set; }
        public double HorasMeta { get; set; }
        public string TipoJornada { get; set; }
        public string ComentariosSistema { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Salida { get; set; }
        public string GrupoName { get { return IdGrupo == 86 ? "Gris" : IdGrupo == 87 ? "Rojo" : IdGrupo == 88 ? "Verde" : "Sin asginar"; } }
    }

    public enum TipoAcceso
    {
        Entrada = 1,
        Salida = 2
    }

}
