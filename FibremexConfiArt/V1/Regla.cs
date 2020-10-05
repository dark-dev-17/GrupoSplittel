using System;
using System.Collections.Generic;
using System.Text;

namespace FibremexConfiArt.V1
{
    public class Regla
    {
        public int IdRegla { get; set; }
        public List<string> Valores { get; set; }
        public TipoRegla TipoRegla { get; set; }
        public List<string> ValoresPara { get; set; }
    }
}
