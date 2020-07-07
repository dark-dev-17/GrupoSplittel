using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Configurables.Configurador
{
    public class ElementCode
    {
        public string Block { get; set; }
        public string Key { get; set; }
        public bool IsFixed { get; set; }
        public bool IsOptional { get; set; }
        public bool IsOpenUser { get; set; }
        public string Type { get; set; }
        public string FixedValue { get; set; }
        public List<OpcionesSelect> Options { get; set; }
    }
}
