using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Configurables.Configurador
{
    public class RestriccionElemento
    {
        public string Restriccion { get; set; }
        public string Block { get; set; }
        public List<Regla> Rules { get; set; }
    }
}
