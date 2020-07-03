using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAPI.Services
{
    public class ConfigurationDinamic
    {
        public string Configuration { get; set; }
        public List<Group> Groups { get; set; }
        public string Expresion { get; set; }
        public string ItemCodeexample { get; set; }
    }
    public class Group
    {
        public string Node { get; set; }
        public string Title { get; set; }
        public string ActualSelected { get; set; }
        public bool ValueFixed { get; set; }
        public bool IsVisible { get; set; }
        public string Value { get; set; }
        public bool IsOpenUser { get; set; }
        public string OpenUserTipeFied { get; set; }
        public bool HasMaskExpresion { get; set; }
        public string MaskExpresion { get; set; }
        public List<ValueGroup> Values { get; set; }
        public string RegularExpresion { get; set; }
    }
    public class ValueGroup
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool HasRestrictions { get; set; }
        public bool IsValid { get; set; }
        public List<Restiction> Restrictions { get; set; }
    }
    public class Restiction
    {
        public string Node { get; set; }
        public List<string> ValuesAcepted { get; set; }
}
}

