using System;
using System.Collections.Generic;
using System.Text;

namespace GPSInformation.Reportes.ProduccionV3
{
    public class PermisosBloq
    {
        public int IdUsuario { get; set; }
        public int IdSubModulo { get; set; }
        public bool Autorization { get; set; }
        public string Descripcion { get; set; }
    }
}
