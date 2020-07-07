using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Configurables.Configurador
{
    public class RestriccionCampoUsuario
    {
        public string BlockKey { get; set; }
        public string Type { get; set; }
        public bool IsRange { get; set; }
        public double RangeFrom { get; set; }
        public double RangeTo { get; set; }
        public bool HasCerosMask { get; set; }
        public int NumberCeros { get; set; }
        public string UnitMesureUser { get; set; }
        public int NumeroMult { get; set; }
    }
}
