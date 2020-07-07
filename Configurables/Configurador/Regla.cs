using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Configurables.Configurador
{
    public class Regla
    {
        public List<string> BlockValues { get; set; }
        public string Type { get; set; }
        public string BlockApply { get; set; }
        public List<string> ValuesAcepted { get; set; }
    }
}
